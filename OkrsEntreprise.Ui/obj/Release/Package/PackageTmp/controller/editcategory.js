okrsapp.controller('editcategory', ['$scope', '$location', '$routeParams', 'localdata', 'goalcategoryservice', function ($scope, $location, $routeParams, localdata, goalcategoryservice) {

    $scope.ErrorMessage = '';
    $scope.editformData = {};
    $scope.Init = function () {
        $scope.getCategory($routeParams.category);
    }
    $scope.updateGoalCategory = function () {

        $scope.loadingImage = true;
        var promise = goalcategoryservice.updateCategory($scope.editformData.id, $scope.editformData.title);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.editcategory = false;
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

    $scope.getCategory = function (id) {
        $scope.loadingImage = true;
        var promise = goalcategoryservice.retrieveCategory(id);
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
                 $location.path('/categories');
             }
         });
    }

    $scope.resetcategory = function () {
        $scope.editformData.title = $scope.title;
        $scope.editGoalCategory = false;
    }
   
    $scope.backtocategories = function () {
        $location.path('/categories');
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