
/**
* Theme: Velonic Admin Template
* Author: Coderthemes
* Nestable Component
*/

!function($) {
    "use strict";

    var Nestable = function() {};

    Nestable.prototype.updateOutput = function (e) {
        var list = e.length ? e : $(e.target),
            output = list.data('output');
        if (window.JSON) {
            output.val(window.JSON.stringify(list.nestable('serialize'))); //, null, 2));
        } else {
            output.val('JSON browser support required for this demo.');
        }
    },
    $('#nestable_list_3').nestable({
        noDragClass: 'dd-nodrag'
    });
    //init
    Nestable.prototype.init = function() {
        // activate Nestable for list

        $('#nestable_list_menu').on('click', function (e) {
            var target = $(e.target),
                action = target.data('action');
            if (action === 'expand-all') {
                $('.dd').nestable('expandAll');
            }
            if (action === 'collapse-all') {
                $('.dd').nestable('collapseAll');
            }
        });

     
    },
    //init
    $.Nestable = new Nestable, $.Nestable.Constructor = Nestable
}(window.jQuery),

//initializing 
function($) {
    "use strict";
    $.Nestable.init();

}(window.jQuery);
$(".dd-nodrag").on("mousedown", function (event) { // mousedown prevent nestable click
    event.preventDefault();
    return false;
});