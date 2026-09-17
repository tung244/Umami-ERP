using Microsoft.AspNetCore.Mvc;

namespace QuanLiKhoHang.Components
{
    public class ButtonViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ButtonViewModel model)
        {
            return View(model);
        }
    }

    public class ButtonViewModel
    {
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = "button";
        public string Style { get; set; } = "primary";
        public string Size { get; set; } = "medium";
        public string Shape { get; set; } = "default";
        public bool Outline { get; set; } = false;
        public bool Soft { get; set; } = false;
        public bool TextOnly { get; set; } = false;
        public string Icon { get; set; } = string.Empty;
        public string IconPosition { get; set; } = "left";
        public string CssClass { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool Disabled { get; set; } = false;
        public bool Loading { get; set; } = false;
        public string OnClick { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string AriaLabel { get; set; } = string.Empty;

        // Business Rules
        public bool RequiresConfirmation { get; set; } = false;
        public string ConfirmationMessage { get; set; } = "Bạn có chắc chắn muốn thực hiện hành động này?";
        public string Permission { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = true;

        // Computed properties
        public string ComputedCssClass
        {
            get
            {
                var classes = new List<string> { "btn" };

                // Base style
                if (TextOnly)
                {
                    classes.Add($"text-{GetColorClass()}");
                }
                else if (Soft)
                {
                    classes.Add($"btn-{GetColorClass()}-100");
                    classes.Add($"text-{GetColorClass()}-600");
                }
                else if (Outline)
                {
                    classes.Add($"btn-outline-{GetColorClass()}");
                }
                else
                {
                    classes.Add($"btn-{GetColorClass()}");
                }

                // Shape
                if (Shape == "pill")
                {
                    classes.Add("rounded-pill");
                }
                else
                {
                    classes.Add("radius-8");
                }

                // Size
                switch (Size)
                {
                    case "small":
                        classes.Add("px-16 py-8");
                        break;
                    case "large":
                        classes.Add("px-24 py-14");
                        break;
                    default: // medium
                        classes.Add("px-20 py-11");
                        break;
                }

                // Icon positioning
                if (!string.IsNullOrEmpty(Icon))
                {
                    classes.Add("d-flex align-items-center gap-2");
                    if (IconPosition == "right")
                    {
                        classes.Add("flex-row-reverse");
                    }
                }

                // Loading state
                if (Loading)
                {
                    classes.Add("disabled");
                }

                // Disabled state
                if (Disabled)
                {
                    classes.Add("disabled");
                }

                // Custom CSS
                if (!string.IsNullOrEmpty(CssClass))
                {
                    classes.Add(CssClass);
                }

                return string.Join(" ", classes);
            }
        }

        private string GetColorClass()
        {
            return Style switch
            {
                "primary" => "primary-600",
                "secondary" => "lilac-600",
                "success" => "success-600",
                "info" => "info-600",
                "warning" => "warning-600",
                "danger" => "danger-600",
                "dark" => "neutral-900",
                "light" => "light-100",
                "link" => "secondary-light",
                _ => "primary-600"
            };
        }

        // Business Rules Validation
        public bool CanRender()
        {
            // Check visibility
            if (!IsVisible) return false;

            // Check permissions (simplified - in real app, check user claims)
            if (!string.IsNullOrEmpty(Permission))
            {
                // TODO: Implement permission checking based on user claims
                // For now, assume all permissions are granted
            }

            return true;
        }

        public string GetConfirmationScript()
        {
            if (!RequiresConfirmation) return string.Empty;

            return $"return confirm('{ConfirmationMessage.Replace("'", "\\'")}');";
        }
    }
}