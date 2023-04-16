okrsapp.controller('edittenant', ['$scope', '$location', '$routeParams', 'localdata', 'tenantservice', function ($scope, $location, $routeParams, localdata, tenantservice) {

    $scope.ErrorMessage = '';
    $scope.editformData = {};
    $scope.Init = function () {
        $scope.gettenant($routeParams.tenant);
    }
    $scope.updatetenant = function () {

        $scope.loadingImage = true;
        var promise = tenantservice.updateTenant($scope.editformData.id, $scope.editformData.name);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.edittenant = false;
              $.Notification.autoHideNotify('success', 'top right', 'Success');
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
                 $scope.resettenant();
             }
         });

    }

    $scope.gettenant = function (id) {
        $scope.loadingImage = true;
        var promise = tenantservice.retrievTenant(id);
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
                 $location.path('/tenants');
             }
         });
    }

    $scope.resettenant = function () {
        $scope.editformData.name = $scope.name;
        $scope.edittenant = false;
    }
   
    $scope.backtotenants = function () {
        $location.path('/tenants');
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