okrsapp.controller('edituser', ['$scope', '$location', '$routeParams', 'localdata', 'userservice',
    function($scope, $location, $routeParams, localdata, userservice) {
        $scope.editformData = {};

        $scope.Init = function() {
            $scope.getuser($routeParams.user);
        }

        $scope.getuser = function (id) {
            alert(id);
        }

        $scope.Init();
    }
]);
