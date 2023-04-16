okrsapp.controller('teams', ['$scope', '$location', '$routeParams', 'localdata', 'teamservice',
    function ($scope, $location, $routeParams, localdata, teamservice) {

        $scope.ErrorMessage = '';
        $scope.teamList = [];

        $scope.NavigateToNewTeam = function () {
            $location.path('/team');
        }

        $scope.editteam = function (id) {
            $location.path('/editteam/' + id);
        }

        $scope.deleteteam = function (id, index) {

            swal({
                title: 'Do you want to delete team - ' + $scope.teamList[index].name,
                text: "You will have to add it again later if required",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            }, function () {

                $scope.loadingImage = true;
                var promise = teamservice.deleteTeam(id);
                promise
                  .success(function (data) {

                      $scope.loadingImage = false;
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'team - ' + $scope.teamList[index].name + ' delete');
                      $scope.teamList.splice(index, 1);
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
            var promise = teamservice.listTeam($scope.searchformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.teamList = data;
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
