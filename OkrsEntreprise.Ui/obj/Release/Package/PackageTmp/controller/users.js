okrsapp.controller('users', ['$scope', '$location', '$routeParams', 'localdata', 'userservice',
    function ($scope, $location, $routeParams, localdata, userservice) {
        

        $scope.ErrorMessage = '';
        $scope.userList = [];

        $scope.NavigateToNewUser = function () {
            $location.path('/user');
        }

        $scope.edituser = function (id) {
            $location.path('/edituser/' + id);
        }

        $scope.deleteuser = function (id, index) {

            swal({
                title: 'Do you want to delete user - ' + $scope.userList[index].UserName,
                text: "You will have to add it again later if required",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            }, function () {

                $scope.loadingImage = true;
                var promise = userservice.deleteUser(id);
                promise
                  .success(function (data) {

                      $scope.loadingImage = false;
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'user - ' + $scope.userList[index].UserName + ' delete');
                      $scope.userList.splice(index, 1);
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

        $scope.processsearchform = function () {

            $scope.loadingImage = true;
            var promise = userservice.listUser($scope.searchformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.userList = data;
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
