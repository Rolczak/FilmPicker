using FilmPicker.Math;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
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
            float percentage = 0f;
            for (int i = 0; i < randomTitleList.Count - 1; i++)
            {
                int currentDurationMs = MathHelper.Lerp(fromDurationMs, toDurationMs, percentage);
                percentage = i / (randomTitleList.Count - 1f);
                var element = await ChangeTextAnimation(randomTitleList[i], currentDurationMs, winnerGrid);
                winnerGrid.Children.Remove(element);
            }
            await ChangeTextAnimation(randomTitleList.Last(), toDurationMs, winnerGrid, true);
        }
        private async Task<TextBlock> ChangeTextAnimation(string text, int waitTime, Panel winnerGrid, bool last = false)
        {    
            CircleEase easing = new();
            easing.EasingMode = EasingMode.EaseInOut;

            DoubleAnimation movingAnimation = new();
            movingAnimation.EasingFunction = easing;
            movingAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(waitTime));
            movingAnimation.From = -100;
            movingAnimation.To = last ? 0 : 100;

            DoubleAnimation opacityAnimation = new();
            opacityAnimation.EasingFunction = easing;
            opacityAnimation.From = 0;
            opacityAnimation.To = 1;
            opacityAnimation.AutoReverse = !last;
            opacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(waitTime / 2));

            var randomTextBlock = new TextBlock();
            randomTextBlock.Text = text;
            randomTextBlock.Style = Application.Current.Resources["HeaderTextBlockStyle"] as Style;
            randomTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            randomTextBlock.RenderTransform = new TranslateTransform();
            winnerGrid.Children.Add(randomTextBlock);

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
