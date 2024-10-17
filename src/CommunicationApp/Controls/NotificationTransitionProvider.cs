using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;

namespace CommunicationApp.Controls
{
    public partial class NotificationTransitionProvider : ItemCollectionTransitionProvider
    {
        protected override bool ShouldAnimateCore(ItemCollectionTransition transition)
        {
            return !transition.Triggers.HasFlag(ItemCollectionTransitionTriggers.LayoutTransition);
        }

        protected override void StartTransitions(IList<ItemCollectionTransition> transitions)
        {
            if (
                transitions.Any(
                    (e) =>
                        e.Triggers.HasFlag(ItemCollectionTransitionTriggers.CollectionChangeReset)
                )
            )
            {
                StartRemoveTransitions(transitions);
                return;
            }

            // the s in linq stands for speed
            var addTransitions = transitions
                .Where(transition => transition.Operation == ItemCollectionTransitionOperation.Add)
                .ToList();
            var removeTransitions = transitions
                .Where(transition =>
                    transition.Operation == ItemCollectionTransitionOperation.Remove
                )
                .ToList();
            var moveTransitions = transitions
                .Where(transition => transition.Operation == ItemCollectionTransitionOperation.Move)
                .ToList();
            StartAddTransitions(addTransitions);
            StartRemoveTransitions(removeTransitions);
            StartMoveTransitions(moveTransitions);
        }

        private static void StartAddTransitions(IList<ItemCollectionTransition> transitions)
        {
            foreach (var transition in transitions)
            {
                var progress = transition.Start();
                MoveElement(
                    progress,
                    (float)(-progress.Element.ActualSize.Y),
                    0.0f,
                    TimeSpan.FromMilliseconds(400)
                );
            }
        }

        private static void StartRemoveTransitions(IList<ItemCollectionTransition> transitions)
        {
            var delay = 0;
            foreach (var transition in transitions)
            {
                var progress = transition.Start();
                ElementCompositionPreview.SetIsTranslationEnabled(progress.Element, true);
                var visual = ElementCompositionPreview.GetElementVisual(progress.Element);
                var compositor = visual.Compositor;

                var anim = compositor.CreateScalarKeyFrameAnimation();
                anim.InsertKeyFrame(
                    1f,
                    visual.Size.X,
                    CompositionEasingFunction.CreateLinearEasingFunction(compositor)
                );
                anim.Duration = TimeSpan.FromMilliseconds(100);
                anim.DelayTime = TimeSpan.FromMilliseconds(delay);
                delay += 16;

                var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                visual.StartAnimation("Translation.X", anim);
                batch.End();

                batch.Completed += delegate
                {
                    progress.Complete();
                };
            }
        }

        private static void StartMoveTransitions(IList<ItemCollectionTransition> transitions)
        {
            foreach (var transition in transitions)
            {
                var progress = transition.Start();
                MoveElement(
                    progress,
                    (float)(transition.OldBounds.Y - transition.NewBounds.Y),
                    0.0f,
                    TimeSpan.FromMilliseconds(250)
                );
            }
        }

        private static void MoveElement(
            ItemCollectionTransitionProgress progress,
            float start,
            float end,
            TimeSpan time
        )
        {
            ElementCompositionPreview.SetIsTranslationEnabled(progress.Element, true);
            var visual = ElementCompositionPreview.GetElementVisual(progress.Element);
            var compositor = visual.Compositor;

            var anim = compositor.CreateScalarKeyFrameAnimation();

            anim.InsertKeyFrame(0.0f, start);
            anim.InsertKeyFrame(
                1.0f,
                end,
                CompositionEasingFunction.CreateCubicBezierEasingFunction(
                    compositor,
                    new(0.55f, 0.55f),
                    new(0, 1)
                )
            );
            anim.Duration = time;

            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            visual.StartAnimation("Translation.Y", anim);
            batch.End();

            batch.Completed += delegate
            {
                progress.Complete();
            };
        }
    }
}
