namespace Ui.WebAssembly.Pages;

public partial class Counter
{
    private int currentCount = 0;
    private bool shouldRender = true;
    private bool shouldRenderValue = true;


    protected override bool ShouldRender()
    {
        return shouldRender;
    }

    private void IncrementCountWithoutRendered()
    {
        shouldRenderValue = shouldRender = false;
        currentCount++;
    }

    private void IncrementCountWithRendered()
    {
        shouldRender = true;
        currentCount++;
    }
}
