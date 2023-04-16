okrsapp.controller('goalcategories', ['$scope', '$location', '$routeParams', 'localdata', 'goalcategoryservice',
    function ($scope, $location, $routeParams, localdata, goalcategoryservice) {

        $scope.loadingImage = false;
        $scope.addformData = {};
        $scope.searchformData = {};
        $scope.ErrorMessage = '';
        $scope.goalCategoryList = [];
        $scope.editformData = {};

        $scope.NavigateToNewCategory = function () {
            $location.path('/category');
        }

        $scope.editcategory = function (id) {
            $location.path('/editcategory/' + id);
        }
       
        $scope.processsearchform = function () {

            $scope.loadingImage = true;
            var promise = goalcategoryservice.listGoalCategory($scope.searchformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.goalCategoryList =data;
                  console.log($scope.goalCategoryList);
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
        $scope.addnewForm = function () {

            $scope.loadingImage = true;
            var promise = goalcategoryservice.createCategory($scope.addformData);
            promise.success(function (data) {
                $scope.loadingImage = false;
                $.Notification.autoHideNotify('success', 'top right', 'Success');
                $scope.resetcategory();
            })
             .error(function (data, status, headers, config) {
                 if (status = 400) {
                     $scope.loadingImage = false;
                     //console.log(data);
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
             });

        }

        $scope.deleteGoalCategory = function (id, index) {

            swal({
                title: 'Do you want to delete category - ' + $scope.goalCategoryList[index].title,
                text: "You will have to add it again later if required",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            }, function () {

                $scope.loadingImage = true;
                var promise = goalcategoryservice.deleteCategory(id);
                promise
                  .success(function (data) {

                      $scope.loadingImage = false;
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'user - ' + $scope.goalCategoryList[index].title + ' delete');
                      $scope.goalCategoryList.splice(index, 1);
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
            });
        }

        $scope.resetcategory = function () {
            $scope.editformData.title = $scope.title;
            $scope.editcategory = false;
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

        $scope.Init = function () {
            $scope.processsearchform();
        }

        $scope.Init();

    }
]);
