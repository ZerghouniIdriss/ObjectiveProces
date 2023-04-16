okrsapp.controller('menu', ['$scope', '$location', 'localdata', 'dataservice', function ($scope, $location, localdata, dataservice) {

    //$scope.username = '';
    //$scope.userimage = '';
    $scope.searchquery = {};

    $scope.isUserLoggedIn = function () {
        var _aid = localdata.getauthenticationID();

        if (_aid.aID != null) {
            return true;
        }
        else
            return false;
    }

    $scope.logOut = function () {

        var _aid = localdata.getauthenticationID();

        if (_aid.aID != null) {
            var promise = dataservice.dologout(_aid.aID);
            promise
               .success(function (data) {
                   localdata.Logout();
                   $location.path('/login');
               })
              .error(function (data, status, headers, config) {
                  if (status == 400) {
                      localdata.Logout();
                      $location.path('/login');
                  }
                  else if (status == 404) {

                      $scope.loadingImage = false;
                      $scope.ErrorMessage = 'Requested link not found';
                  }

                  localdata.Logout();
                  $location.path('/login');
                  $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);

              });
        }
        else {
            localdata.Logout();
            $location.path('/login');
        }
    }
    $scope.onSuccess = function () {
        $location.path('/objectives');
    }

    $scope.Init = function () {

        var userData = localdata.getuserData().un;

        if (userData != undefined) {

            $scope.username = userData.userName;
            $scope.userimage = userData.userImage;
            $scope.userId = localdata.getuserData().un.userId;
        }

        $scope.$on('userDataChanged', $scope.UserNameChanged);
    }

    $scope.UserNameChanged = function () {

        $scope.username = localdata.getuserData().un.userName;
        $scope.userimage = localdata.getuserData().un.userImage;
        $scope.userId = localdata.getuserData().un.userId;

    }

    $scope.searchObjective = function () {

        if ($scope.searchquery.text != '') {
            $location.path('/objectives/' + $scope.searchquery.text);
        }
        $scope.searchquery.text = '';
    }

    $scope.navigateToNewGoal = function () {
        $location.path('/goal');
    }
    $scope.Init();
}]);