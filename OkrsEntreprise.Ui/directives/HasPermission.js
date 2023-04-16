okrsapp.directive('hasPermission', ['localdata', 'managepermission', function (localdata, managepermission) {


    return {
        link: function (scope, element, attrs, ctrl) {

            scope.$watch(attrs.userName, function (value) {

                var value = attrs.hasPermission.trim();

                if (!managepermission.isPermitted(value)) {
                    element.hide();
                }
                else {
                    element.show();
                }

            });
        }
    }

}]);