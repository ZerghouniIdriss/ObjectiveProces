okrsapp.controller('team', ['$scope', '$location', '$routeParams', 'localdata', 'teamservice',
    function ($scope, $location, $routeParams, localdata, teamservice) {

        $scope.addformData = {};

        $scope.assigneeList = []
        $scope.addformData.Users = [];

        $scope.Init = function () {
            $scope.getuser();
        }

        $scope.addnewForm = function () {

            $scope.loadingImage = true;
            var promise = teamservice.createTeam($scope.addformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', $scope.addformData.Name + ' added to database');

                  $scope.addformData = {};
                  $scope.addformData.Users = [];

                  angular.forEach($scope.assigneeList, function (value, key) {
                      value.ticked = false;
                  });


              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     //console.log(data);
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Requested link not found';
                 }

                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);

             });

        }

        $scope.getuser = function () {

            $scope.loadingImage = true;
            var promise = teamservice.userAssigneeList($scope.addformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.userList = data;
                  $scope.prepareassignees();
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     //console.log(data);
                     $scope.ErrorMessage = scope.parseErrors(data);
                 }
                 else if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', scope.ErrorMessage);

             });

        }

        $scope.prepareassignees = function () {
            $scope.assigneeList = [];
            $scope.assigneeList.push({
                name: '<strong>Assignees</strong>',
                msGroup: true
            });

            if ($scope.userList.length > 0) {

                $scope.assigneeList.push({
                    name: '<strong>Persons</strong>',
                    msGroup: true
                });

                angular.forEach($scope.userList, function (value, key) {

                    $scope.assigneeList.push({
                        icon: '<img  src="' + value.Avatar + '" />',
                        name: value.name,
                        id: value.id,
                        email: value.Email,
                        isteam: false,
                        ticked: false
                    });
                });

                $scope.assigneeList.push({
                    msGroup: false
                });
            }

            $scope.assigneeList.push({
                msGroup: false
            });

        }

        $scope.Init();

    }
]);