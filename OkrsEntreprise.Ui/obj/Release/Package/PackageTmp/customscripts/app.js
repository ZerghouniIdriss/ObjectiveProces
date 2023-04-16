var okrsapp = angular.module('okrsapp', ['ngRoute', 'ui.bootstrap', 'ngMessages', 'isteven-multi-select', '720kb.datepicker', 'ngSanitize']);



angular.element(document.body).bind('click', function (e) {
    if ($(e.target).parents('.popover').length) {
        return;
    }

    // Added code to display popover on clicking of entire progress bar
    if ($(e.target).hasClass('progress-bar'))
        e.target = e.target.parentNode;

    var popups = document.querySelectorAll('.popover');
    if (popups) {
        for (var i = 0; i < popups.length; i++) {
            var popup = popups[i];
            var popupElement = angular.element(popup);
            if (popupElement[0].previousSibling != e.target) {
                popupElement.scope().$parent.isOpen = false;
                popupElement.remove();
            }
        }
    }
});
