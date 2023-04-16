okrsapp.controller('editteam', ['$scope', '$location', '$routeParams', 'localdata', 'teamservice',
    function($scope, $location, $routeParams, localdata, teamservice) {
        $scope.editformData = {};
        $scope.editname = false;
        $scope.editdes = false;
        $scope.oldname = '';
        $scope.olddes = '';
        $scope.ErrorMessage = '';
        $scope.teamList = [];
        $scope.assigneeList = [];
        $scope.userList = [];

        $scope.Init = function() {
            $scope.getusers();
            $scope.getteam($routeParams.team);
            // $scope.getteams(); 
        }

        $scope.updateTeam = function() {
            return;
            //above are dummy, to be removed
            $scope.loadingImage = true;
            var promise = teamservice.editteam($scope.editformData);
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    alert($scope.editformData.Name + ' was updated')
                    if ($routeParams.ret != undefined) {
                        var pvalues = $location.path().split('/');
                        var newpath = '';
                        for (var i = 1; i < pvalues.length - 1; i++) {
                            newpath = newpath + '/' + pvalues[i];
                        }
                        $location.path(newpath);
                    } else {
                        $location.path('/teams');
                    }
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        //console.log(data);
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        }

        $scope.prepareassignees = function() {
            $scope.assigneeList = [];      
            if ($scope.userList.length > 0) {
                $scope.assigneeList.push({
                    name: '<strong>Users</strong>',
                    msGroup: true
                });
                angular.forEach($scope.userList, function(value, key) {
                    $scope.assigneeList.push({
                        icon: '<img  src="/images/user.png" />',
                        name: value.UName,
                        id: value.id,
                        isteam: false,
                        ticked: false
                    });
                });

                $scope.assigneeList.push({
                    msGroup: false
                });
            }
        }

        $scope.getteam = function(id) { 
            $scope.loadingImage = true;
            $scope.editformData = {
                'Id': 0,
                'Name': 'example team Name',
                'Description': 'example team Description',
                'Users': 'example users'
            };
            //$scope.checkassign();
            $scope.prepareassignees();
            $scope.loadingImage = false;
            return;
            //above are dummy data, to be removed

            $scope.loadingImage = true;
            var promise = dataservice.retrievteam(id);
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    $scope.editformData = data;
                    $scope.checkassign();
                })
                .error(function(data, status, headers, config) {
                    if (status == 400) {
                        $scope.loadingImage = false;
                        //console.log(data);
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    } else if (status == 404) {
                        $location.path('/teams');
                    }
                });
        }


        $scope.getusers = function() {
            //fake get user
            $scope.loadingImage = true;
            $scope.userList = [{
                'id': 0,
                'FName': 'willialn',
                'LName': 'brown',
                'UName': 'wilbrownwa',
                'Email': 'u1@e.c',
                'Password': '123321'
            }, {
                'id': 0,
                'FName': 'taylor',
                'LName': 'jack',
                'UName': 'tayjackiga',
                'Email': 'u1@e.c',
                'Password': '123321'
            }, {
                'id': 0,
                'FName': 'lopexz',
                'LName': 'clarop',
                'UName': 'claropzaa',
                'Email': 'u1@e.c',
                'Password': '123321'
            }];
            $scope.loadingImage = false;
            return;
            //above are dummy data, to be removed

            $scope.loadingImage = true;
            var promise = teamservice.searchuser($scope.addformData);
            promise
                .success(function(data) {
                    $scope.loadingImage = false;
                    $scope.userList = data;
                    $scope.prepareassignees();
                })
                .error(function(data, status, headers, config) {
                    if (status = 400) {
                        $scope.loadingImage = false;
                        //console.log(data);
                        $scope.ErrorMessage = $scope.parseErrors(data);
                    }
                });
        }


        $scope.backtoteams = function() {
            $location.path('/teams');
        }


        $scope.parseErrors = function(response) {
            var errors = [];
            for (var key in response.ModelState) {
                for (var i = 0; i < response.ModelState[key].length; i++) {
                    errors.push(response.ModelState[key][i]);
                }
            }
            return errors.toString();
        }
        $scope.currentUser = function(item) {
            return localdata.getuserName().un == item.user.UName;
        }

        $scope.assignteam = function() {
            var promise = null;
            if ($scope.isassign) {
                promise = dataservice.removeme($scope.editformData);
                promise
                    .success(function(data) {
                        $scope.loadingImage = false;
                        var idx = -1;
                        for (var i = 0; i < $scope.editformData.assignees.length; i++) {
                            if ($scope.editformData.assignees[i].name == localdata.getuserName().un) {
                                idx = i;
                            }
                        }
                        $scope.editformData.assignees.splice(idx, 1);
                        $scope.checkassign();
                    })
                    .error(function(data, status, headers, config) {
                        if (status == 400) {
                            $scope.loadingImage = false;
                            $scope.ErrorMessage = $scope.parseErrors(data);
                        } else if (status == 404) {
                            $scope.loadingImage = false;
                            $scope.ErrorMessage = 'Not Found';
                        }
                    });
            } else {
                promise = dataservice.assignme($scope.editformData);

                promise
                    .success(function(data) {
                        $scope.loadingImage = false;
                        var usr = { "id": 0, "name": localdata.getuserName().un, "isteam": false }
                        $scope.editformData.assignees.push(usr);
                        $scope.checkassign();
                    })
                    .error(function(data, status, headers, config) {
                        if (status == 400) {
                            $scope.loadingImage = false;
                            $scope.ErrorMessage = $scope.parseErrors(data);
                        } else if (status == 404) {

                            $scope.loadingImage = false;
                            $scope.ErrorMessage = 'Not Found';

                        }

                    });
            }
        }
        $scope.Init();
    }
]);
