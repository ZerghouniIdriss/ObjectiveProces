okrsapp.controller('employeesessions', ['$scope', '$location', 'localdata', 'dataservice', function ($scope, $location, localdata, dataservice) {

    $scope.ErrorMessage = '';
    $scope.sessionList = [];

    $scope.NavigateToNewSession = function () {
        $location.path('/employeesession');
    }

    $scope.editsession = function (id) {
        $location.path('/employeesessiondetail/' + id);
    }

    $scope.deletesession = function (id, index) {

        swal({
            title: 'Do you want to delete session for Attendee - ' + $scope.sessionList[index].Attendee.name,
            text: "You will have to add it again later if required",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: true
        }, function () {

            $scope.loadingImage = true;
            var promise = dataservice.deleteEmpEvalSession(id);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Employee session delete');
                  $scope.sessionList.splice(index, 1);
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

    $scope.getsessions = function () {

        $scope.loadingImage = true;
        var promise = dataservice.searchEmpEvalSessions();
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
        $scope.getsessions();
    }

    $scope.Init();

}]);