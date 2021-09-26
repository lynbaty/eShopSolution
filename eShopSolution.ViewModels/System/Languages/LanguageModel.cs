using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Languages
{
    public class LanguageModel
    {
        public List<LanguageDto> Languages { set; get; }

        public string CurrentLanguageId { set; get; } = "vi";
    }
}