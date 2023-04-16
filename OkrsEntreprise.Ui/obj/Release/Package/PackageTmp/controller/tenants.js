okrsapp.controller('tenants', ['$scope', '$location', '$routeParams', 'localdata', 'tenantservice',
    function ($scope, $location, $routeParams, localdata, tenantservice) {

        $scope.loadingImage = false;
        $scope.addformData = {};
        $scope.searchformData = {};
        $scope.ErrorMessage = '';
        $scope.tenantList = [];
        $scope.editformData = {};
        $scope.NavigateToNewTenant = function () {
            $location.path('/tenant');
        }

        $scope.edittenant = function (id) {
            $location.path('/edittenant/'+id);
        }

        $scope.deletetenant = function (id, index) {

            swal({
                title: 'Do you want to delete user - ' + $scope.tenantList[index].name,
                text: "You will have to add it again later if required",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            }, function () {

                $scope.loadingImage = true;
                var promise = tenantservice.deleteTenant(id);
                promise
                  .success(function (data) {

                      $scope.loadingImage = false;
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'user - ' + $scope.tenantList[index].UserName + ' delete');
                      $scope.tenantList.splice(index, 1);
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
            var promise = tenantservice.listTenant($scope.searchformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.tenantList = data;
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
            var promise = tenantservice.createTenant($scope.addformData);
            promise.success(function (data) {
                $scope.loadingImage = false;
                $.Notification.autoHideNotify('success', 'top right', 'Success');
                $scope.resettenant();
                })
             .error(function (data, status, headers, config) {
                 if (status = 400) {
                     $scope.loadingImage = false;
                     //console.log(data);
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
             });

        }


        $scope.resettenant = function () {
            $scope.editformData.name = $scope.name;
            $scope.edittenant = false;
        }

        $scope.backtotenants = function() {
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

        $scope.Init = function () {
            $scope.processsearchform();
        }

        $scope.Init();

    }
]);
