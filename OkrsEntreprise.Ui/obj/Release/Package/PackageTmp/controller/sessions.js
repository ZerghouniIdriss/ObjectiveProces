okrsapp.controller('sessions', ['$scope', '$location', '$routeParams', 'localdata', 'dataservice', function ($scope, $location, $routeParams, localdata, dataservice) {

    $scope.ErrorMessage = '';
    $scope.sessionList = [];

    $scope.NavigateToNewSession = function () {
        $location.path('/oneOnOneSession');
    }

    $scope.editsession = function (id) {
        $location.path('/sessiondetail/' + id);
    }

    $scope.getsessions = function () {

        $scope.loadingImage = true;
        var promise = dataservice.searchSessions();
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.sessionList = data;
              //console.log(data);
              //console.log(JSON.stringify(data[0]));
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else
                 if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Requested link not found';
                 }
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
         });

    }

    $scope.Init = function () {
        $scope.getsessions();
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

    $scope.Init();
}]);