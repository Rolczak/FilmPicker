using FilmPicker.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Animations
{
    public class PickingAnimation
    {
        private readonly List<string> randomTitleList;
        private readonly int fromDurationMs;
        private readonly int toDurationMs;

        public PickingAnimation(List<string> randomTitleList, int fromDurationMs, int toDurationMs)
        {
            this.randomTitleList = randomTitleList;
            this.fromDurationMs = fromDurationMs;
            this.toDurationMs = toDurationMs;
        }

        public async Task Animate(Panel winnerGrid)
        {
            Debug.WriteLine("Starting picking animation");

            float percentage = 0f;
            for (int i = 0; i < randomTitleList.Count - 1; i++)
            {
                int currentDurationMs = MathHelper.Lerp(fromDurationMs, toDurationMs, percentage);
                percentage = i / (randomTitleList.Count - 1f);
                var element = await ChangeTextAnimation(randomTitleList[i], currentDurationMs, winnerGrid);
                winnerGrid.Children.Remove(element);
            }
            await ChangeTextAnimation(randomTitleList.Last(), toDurationMs, winnerGrid, true);

            Debug.WriteLine("Picking animation finished");
        }
        private async Task<TextBlock> ChangeTextAnimation(string text, int waitTime, Panel winnerGrid, bool last = false)
        {
            var panelHeight = winnerGrid.ActualHeight;

            CircleEase easing = new()
            {
                EasingMode = EasingMode.EaseInOut
            };

            TextBlock randomTextBlock = new()
            {
                Text = text,
                Style = Application.Current.Resources["HeaderTextBlockStyle"] as Style,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                RenderTransform = new TranslateTransform(),
            };
            winnerGrid.Children.Add(randomTextBlock);

            DoubleAnimation movingAnimation = new()
            {
                EasingFunction = easing,
                Duration = new Duration(TimeSpan.FromMilliseconds(waitTime)),
                From = -panelHeight / 2,
                To = last ? 0 : (panelHeight - randomTextBlock.FontSize) / 2
            };

            DoubleAnimation opacityAnimation = new()
            {
                EasingFunction = easing,
                From = 0,
                To = 1,
                AutoReverse = !last,
                Duration = new Duration(TimeSpan.FromMilliseconds(waitTime / 2))
            };

            Storyboard.SetTarget(movingAnimation, randomTextBlock.RenderTransform);
            Storyboard.SetTargetProperty(movingAnimation, "Y");
            Storyboard.SetTarget(opacityAnimation, randomTextBlock);
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

            Storyboard storyboard = new();
            storyboard.Children.Add(movingAnimation);
            storyboard.Children.Add(opacityAnimation);
            storyboard.Begin();
            await Task.Delay(waitTime);

            return randomTextBlock;
        }

    }
}
