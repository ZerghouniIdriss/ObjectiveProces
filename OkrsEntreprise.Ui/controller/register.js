okrsapp.controller('register', ['$scope', '$location', 'localdata', 'dataservice', function ($scope, $location, localdata, dataservice) {

    $scope.loadingImage = false;
    $scope.ErrorMessage = '';
    $scope.formData = {};

    $scope.processForm = function () {
        $scope.loadingImage = true;
        //$scope.formData.aid = localdata.getauthenticationID().aID;
        var promise = dataservice.doregistration($scope.formData);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $location.path('/login');
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 //$scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 //$scope.loadingImage = false;
                 $scope.ErrorMessage = 'Requested link not found';
             }
             else
             {
                 $scope.ErrorMessage = 'Could not connect. Please try again later';
             }
             $scope.loadingImage = false;
             $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);

         });
    }

    //$scope.Init = function () {
    //};

    //$scope.Init();

    //$scope.$on('$routeChangeStart', function () {
    //    $scope.loadingImage = true;
    //});
    //$scope.$on('$routeChangeSuccess', function () {
    //    $scope.loadingImage = false;
    //});

    $scope.parseErrors = function (response) {
        var errors = [];
        for (var key in response.ModelState) {
            for (var i = 0; i < response.ModelState[key].length; i++) {
                errors.push(response.ModelState[key][i]);
            }
        }
        return errors.toString();
    }
}]);