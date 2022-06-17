using SampleWebApp;

namespace TestProj
{
    public class CalculationTest
    {
        [Theory]
        [InlineData(4,3,7)]
        [InlineData(5,3,8)]
        [InlineData(9,3,12)]
        public void AddShouldWork(double x, double y, double expected)
        {
            double actual = Calculation.Add(x,y);
            Assert.Equal(expected, actual);
        }
    }
}