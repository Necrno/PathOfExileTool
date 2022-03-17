using Microsoft.AspNetCore.Components;

namespace PathOfExileShoppingTool.Pages.Components
{
    public class CardComponent : ComponentBase
    {
        [Parameter]
        public string MainTitle { get; set; }

        [Parameter]
        public string ParagraphTitle { get; set; }

        [Parameter]
        public string CardText { get; set; }

        [Parameter]
        public string PageUrl { get; set; }

        [Parameter]
        public string PageTitle { get; set; }
    }
}
