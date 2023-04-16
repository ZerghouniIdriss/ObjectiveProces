okrsapp.controller('editstatus', ['$scope', '$location', '$routeParams', 'localdata', 'goalstatusservice', function ($scope, $location, $routeParams, localdata, goalstatusservice) {

    $scope.ErrorMessage = '';
    $scope.editformData = {};
    $scope.Init = function () {
        $scope.getStatus($routeParams.status);
    }
    $scope.updateGoalStatus = function () {

        $scope.loadingImage = true;
        var promise = goalstatusservice.updateStatus($scope.editformData.id, $scope.editformData.title);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.editstatus = false;
              $.Notification.autoHideNotify('success', 'top right', 'Success');
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
                 $scope.resetcategory();
             }
         });

    }

    $scope.getStatus = function (id) {
        $scope.loadingImage = true;
        var promise = goalstatusservice.retrieveStatus(id);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.editformData = data;

              
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $location.path('/status');
             }
         });
    }

    $scope.resetstatus = function () {
        $scope.editformData.title = $scope.title;
        $scope.editGoalStatus = false;
    }
   
    $scope.backtostatus = function () {
        $location.path('/status');
    };
   

    $scope.parseErrors = function (response) {
        var errors = [];
        for (var key in response.ModelState) {
            for (var i = 0; i < response.ModelState[key].length; i++) {
                errors.push(response.ModelState[key][i]);
            }
        }
        return errors.toString();
    }



    $scope.Init();

}]);