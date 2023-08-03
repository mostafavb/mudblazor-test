using Ui.WebAssembly.Pages;

namespace Test.bUnit.Pages;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class CounterTests : TestContext
{
    [Fact]
    public void Counter_Starts_At_Zero()
    {
        var cut = RenderComponent<Counter>();

        cut.Find("p").MarkupMatches("<p class=\"mud-typography mb-4 mud-typography-body1\" >Current count: 0</p>");
    }

    [Fact]
    public void Should_Render_Button_Click()
    {
        var cut = RenderComponent<Counter>();

        var btnShouldRender = cut.FindAll("button")[0];

        btnShouldRender.MarkupMatches("<button diff:ignoreAttributes><span diff:ignoreAttributes>Click with rendered</span></button>");
        btnShouldRender.Click();

        cut.Find("p").MarkupMatches("<p diff:ignoreAttributes>Current count: 0</p>");
    }

    [Fact]
    public void Shouldnot_Render_Button_Click()
    {
        var cut = RenderComponent<Counter>();

        var btnShouldnotRender = cut.FindAll("button")[1];

        btnShouldnotRender.MarkupMatches("<button diff:ignoreAttributes><span diff:ignoreAttributes>Click without rendered</span></button>");

        btnShouldnotRender.Click();
        cut.Find("p").MarkupMatches("<p diff:ignoreAttributes>Current count: 0</p>");
    }
}
