
using NUnit.Framework;

public class DashCooldownLogicTests
{
    [Test]
    public void Dash_Is_ReEnabled_After_3_Seconds()
    {
        var logic = new DashCooldown();
        logic.UseDash();
        logic.Tick(3.1f);

        Assert.IsTrue(logic.CanDashAgain());
    }

    [Test]
    public void Dash_Is_Disabled_Before_3_Seconds()
    {
        var logic = new DashCooldown();
        logic.UseDash();
        logic.Tick(2.9f);

        Assert.IsFalse(logic.CanDashAgain());
    }
}
