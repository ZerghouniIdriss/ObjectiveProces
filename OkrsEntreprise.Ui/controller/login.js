okrsapp.controller('login', ['$scope', '$location', 'localdata', 'dataservice', function ($scope, $location, localdata, dataservice) {

    $scope.loadingImage = false;
    $scope.ErrorMessage = '';
    $scope.formData = {};

    $scope.processForm = function () {

        $scope.loadingImage = true;
        $scope.formData.grant_type = 'password';

        var promise = dataservice.dologin($scope.formData);

        promise.done(function (data) {
            //console.log(data.access_token);
            localdata.setauthenticationID(data.access_token);
            localdata.setuserData(data);
            //console.log(data);
            $scope.$apply(function () {
                $scope.loadingImage = false;
                $location.path('/dashboard');
            });

        }).fail(showError);

    }

    function showError(jqXHR) {
        $scope.$apply(function () {
            $scope.loadingImage = false;
            if (jqXHR.responseText == '') {
                $scope.ErrorMessage = 'Could not connect. Please try again later';
            }
            else {

                $scope.ErrorMessage = JSON.parse(jqXHR.responseText).error_description;
            }
            $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
        });
    }

    $scope.$on('$routeChangeStart', function () {
        $scope.loadingImage = true;
    });
    $scope.$on('$routeChangeSuccess', function () {
        $scope.loadingImage = false;
    });
}]);