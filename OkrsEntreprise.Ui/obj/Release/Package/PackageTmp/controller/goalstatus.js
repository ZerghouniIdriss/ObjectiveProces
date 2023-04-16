okrsapp.controller('goalstatus', ['$scope', '$location', '$routeParams', 'localdata', 'goalstatusservice',
    function ($scope, $location, $routeParams, localdata, goalstatusservice) {

        $scope.loadingImage = false;
        $scope.addformData = {};
        $scope.searchformData = {};
        $scope.ErrorMessage = '';
        $scope.goalStatusList = [];
        $scope.editformData = {};

        $scope.NavigateToNewStatus = function () {
            $location.path('/newstatus');
        }

        $scope.editcategory = function (id) {
            $location.path('/editstatus/' + id);
        }
       
        $scope.processsearchform = function () {

            $scope.loadingImage = true;
            var promise = goalstatusservice.listGoalStatus($scope.searchformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.goalStatusList = data;
                  console.log($scope.goalStatusList);
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
            var promise = goalstatusservice.createStatus($scope.addformData);
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

        $scope.deleteGoalStatus = function (id, index) {

            swal({
                title: 'Do you want to delete status - ' + $scope.goalStatusList[index].title,
                text: "You will have to add it again later if required",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            }, function () {

                $scope.loadingImage = true;
                var promise = goalstatusservice.deleteStatus(id);
                promise
                  .success(function (data) {

                      $scope.loadingImage = false;
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'user - ' + $scope.goalStatusList[index].title + ' delete');
                      $scope.goalStatusList.splice(index, 1);
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

        $scope.resetstatus = function () {
            $scope.editformData.title = $scope.title;
            $scope.editcategory = false;
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

        $scope.Init = function () {
            $scope.processsearchform();
        }

        $scope.Init();

    }
]);
