okrsapp.controller('map', ['$scope', '$location', '$routeParams', 'localdata', 'goalservice',
    function ($scope, $location, $routeParams, localdata, goalservice) {

        $scope.loadingImage = false;
  
        $scope.searchformData = {};
        $scope.ErrorMessage = '';
        $scope.goalList = [];
      
        $scope.processsearchform = function () {

            $scope.loadingImage = true;
            var promise = goalservice.listGoal();
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.goalList = data;
                  console.log($scope.goalList);
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
             });

        }
    

        $scope.parseErrors = function (response) {
            var errors = [];
            for (var key in response.ModelState) {
                for (var i = 0; i < response.ModelState[key].length; i++) {
                    errors.push(response.ModelState[key][i]);
                }
            }
            return errors.toString();
        }

        $scope.Init = function () {
            $scope.processsearchform();
        }

        $scope.Init();

    }
]);
