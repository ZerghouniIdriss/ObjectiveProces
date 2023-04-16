okrsapp.controller('userprofile', ['$scope', '$rootScope', '$location', '$routeParams', 'localdata', 'userservice', '$filter',
    function ($scope, $rootScope, $location, $routeParams, localdata, userservice, $filter) {

        $scope.editformData = {};
        $scope.editformData.Roles = [];
        $scope.editformData.Manager = { Id: 0 };
        $scope.userList = [];
        $scope.assignees = [];
        $scope.uploadedImage = null;
        $scope.newUserRole = 0;
        $scope.IsUserAdmin = false;
        $scope.CanEditDetails = false;
        $scope.Init = function () {
            $scope.getUser($routeParams.user);

            $scope.getteams();
            $scope.getRoles();
            $scope.getuserList();
            $("#roleSelect").on("change", function (e) {
                $scope.$apply(function () {

                    if (e.val == '') {

                        $scope.newUserRole = 0;
                    }
                    else {

                        $scope.newUserRole = parseInt(e.val);
                    }
                });

            });
            var userRoles = localdata.getuserData().un.userRoles.split(',');
            angular.forEach(userRoles, function (value, key) {
                if (value == 3 || value == 4) {
                    $scope.IsUserAdmin = true;
                    $scope.CanEditDetails = true;
                }
            });
            //$('.chart').easyPieChart({
            //    barColor: '#34c73b',
            //    size: 100
            //});
            $('.chart').easyPieChart({
                easing: 'easeOutBounce',
                barColor: '#8CC152',
                lineWidth: 3,
                onStep: function (from, to, percent) {
                    $(this.el).find('.percent').text(Math.round(percent));
                }
            });
        };

        $scope.isFNEdit = false;
        $scope.isLNEdit = false;
        $scope.isELEdit = false;
        $scope.isUMEdit = false;

        //tab
        $scope.tab = 1;
        $scope.setTab = function (newTab) {
            $scope.tab = newTab;
        };
        $scope.isSet = function (tabNum) {
            return $scope.tab === tabNum;
        };
        //tab

        $scope.getteams = function () {

            $scope.loadingImage = true;
            var promise = userservice.searchteam($scope.addformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.teamList = data;
                  $scope.prepareassignees();
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

        $scope.getuserList = function () {

            $scope.loadingImage = true;
            var promise = userservice.listUser($scope.addformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  //console.log(data);
                  $scope.userList = data;
                  setUserManager();
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
             });

        }

        function setUserManager() {
            $scope.editformData.Manager = ($filter('filter')($scope.userList, { UserName: $scope.editformData.ManagerUserName }))[0];
        }

        $scope.getUser = function (id) {

            $scope.loadingImage = true;
            var promise = userservice.retrievUser(id);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.editformData = data;
                  $scope.getOverallProgress();
                  $scope.getAligned();
                  $scope.getRoleFromSelect();
                  if (localdata.getuserData().un.userId == $scope.editformData.Id)
                      $scope.CanEditDetails = true;
                  else
                      $('#multiSelectRoles *').prop('disabled', true);
                  setUserManager();

              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
             });
        }

        $scope.getRoles = function () {

            $scope.loadingImage = true;
            var promise = userservice.getRoles();
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.roleList = data;
                  $scope.getRoleFromSelect();
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

        $scope.addteam = function () {

            if ($scope.assignees.length > 0) {

                var found = $scope.teamExists($scope.assignees);

                if (!found) {

                    var inputData = { Id: $scope.editformData.Id, Teams: $scope.assignees };
                    $scope.loadingImage = true;
                    var promise = userservice.assignTeams(JSON.stringify(inputData));
                    promise
                      .success(function (data) {

                          $scope.loadingImage = false;
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Team assigned to user')
                          angular.forEach($scope.assignees, function (item) {

                              for (var i = 0; i < $scope.teamList.length; i++) {
                                  if ($scope.teamList[i].id == item.id) {

                                      $scope.editformData.Teams.push($scope.teamList[i])

                                  }
                              }
                          });

                          $scope.assignees = [];
                          angular.forEach($scope.assigneeList, function (item) {
                              item.ticked = false;
                          });
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {
                             $scope.loadingImage = false;
                             $scope.ErrorMessage = $scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             $scope.ErrorMessage = 'Requested link not found';
                         }
                         $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                     });
                }
                else {
                    $scope.ErrorMessage = 'User already found in team';
                    $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
                }

            }
            else {
                $scope.ErrorMessage = 'Select team(s) to assign to user';
                $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
            }
        }

        $scope.teamExists = function (teams) {

            var found = false;

            angular.forEach($scope.editformData.Teams, function (value, key) {

                for (var i = 0; i < teams.length; i++) {

                    if (value.id == teams[i].id) {
                        found = true;
                        return found;
                    }

                }
            });

            return found;
        }

        $scope.deleteTeam = function (index) {

            swal({
                title: 'Do you want to delete user- ' + $scope.editformData.UserName + ' from team - ' + $scope.editformData.Teams[index].name,
                text: "You will have to add it again later if required",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            }, function () {

                $scope.loadingImage = true;

                var inputTeam = [];
                inputTeam.push($scope.editformData.Teams[index]);
                var inputData = { Id: $scope.editformData.Id, Teams: inputTeam };

                var promise = userservice.deleteTeam(JSON.stringify(inputData));
                promise
                  .success(function (data) {
                      $scope.editformData.Teams.splice(index, 1);
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'Team deleted');
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {

                         $scope.loadingImage = false;
                         $scope.ErrorMessage = $scope.parseErrors(data);
                     }
                     else if (status == 404) {
                         $scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                 });
            });
        }

        $scope.addRole = function () {

            var found = $scope.roleExists($scope.newUserRole);

            if (!found) {

                if ($scope.newUserRole != 0 && $scope.newUserRole != '') {

                    var inputRole = [];
                    var rl = { Id: $scope.newUserRole };
                    inputRole.push(rl);

                    var inputData = { id: $scope.editformData.Id, Roles: inputRole };

                    var promise = userservice.assignRole(JSON.stringify(inputData));
                    promise
                      .success(function (data) {

                          angular.forEach($scope.roleList, function (value, key) {
                              if (value.Id == $scope.newUserRole)
                                  $scope.editformData.Roles.push(value);
                          });

                          $scope.newUserRole = 0;
                          roleSelect.val('').trigger("change");
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'Role added');
                      })
                     .error(function (data, status, headers, config) {
                         if (status == 400) {

                             $scope.loadingImage = false;
                             $scope.ErrorMessage = $scope.parseErrors(data);
                         }
                         else if (status == 404) {
                             $scope.ErrorMessage = 'Requested link not found';
                         }
                         $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                     });
                }
                else {
                    $scope.ErrorMessage = 'Select role to assign to user';
                    $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                }
            }
            else {
                $scope.ErrorMessage = 'Role already found in user';
                $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
            }
        }

        $scope.roleExists = function (roleId) {

            var found = false;

            angular.forEach($scope.editformData.Roles, function (value, key) {

                if (value.Id == roleId) {
                    found = true;
                    return found;
                }

            });

            return found;
        }

        $scope.deleteRole = function (index) {

            swal({
                title: 'Do you want to delete role- ' + $scope.editformData.Roles[index].Name + ' from from - ' + $scope.editformData.UserName,
                text: "You will have to add it again later if required",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            }, function () {

                $scope.loadingImage = true;

                var inputRole = [];
                inputRole.push($scope.editformData.Roles[index]);
                var inputData = { Id: $scope.editformData.Id, Roles: inputRole };

                var promise = userservice.deleteRole(JSON.stringify(inputData));
                promise
                  .success(function (data) {
                      $scope.editformData.Roles.splice(index, 1);
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'Role deleted');
                  })
                 .error(function (data, status, headers, config) {
                     if (status == 400) {

                         $scope.loadingImage = false;
                         $scope.ErrorMessage = $scope.parseErrors(data);
                     }
                     else if (status == 404) {
                         $scope.ErrorMessage = 'Requested link not found';
                     }
                     $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                 });
            });
        }

        $scope.getFNeditTemp = function () {
            if ($scope.isFNEdit) {
                return 'FNedit';
            }
            else {
                return 'FNdisplay';
            }
        }

        $scope.setFNeditTemp = function () {
            $scope.editformData.NewFirstName = $scope.editformData.FirstName
            $scope.isFNEdit = true;
        }

        $scope.resetFirstName = function () {
            $scope.isFNEdit = false;
        }

        $scope.updateFName = function (name) {

            var inputData = { Id: $scope.editformData.Id, FirstName: name };
            $scope.loadingImage = true;
            var promise = userservice.editFirstName(JSON.stringify(inputData));
            promise
              .success(function (data) {
                  $scope.loadingImage = false;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'First Name Updated');
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
             });

        }

        $scope.updateFirstName = function () {

            var inputData = { Id: $scope.editformData.Id, FirstName: $scope.editformData.NewFirstName };
            $scope.loadingImage = true;
            var promise = userservice.editFirstName(JSON.stringify(inputData));
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.editformData.FirstName = $scope.editformData.NewFirstName;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'First Name Updated');
                  $scope.resetFirstName();
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
                 $scope.resetFirstName();
             });

        }

        $scope.$on('updateProgress', function (event, args) {
            console.log("updateProgress");
            $scope.getOverallProgress();
            // do what you want to do
        });

        $scope.$on('ObjectiveOpenCloseChanged', function (event, goal) {
            if (!goal.isopen) {
                var indx;

                angular.forEach($scope.editformData.Objective, function (value, key) {
                    if (value.id == goal.id) {
                        indx = key;
                    }
                });

                $scope.editformData.Objective.splice(indx, 1);
            }
        });

        $scope.getLNeditTemp = function () {

            if ($scope.isLNEdit) { return 'LNedit'; }
            else
            {
                return 'LNdisplay';
            }
        }

        $scope.setLNeditTemp = function () {
            $scope.editformData.NewLastName = $scope.editformData.LastName
            $scope.isLNEdit = true;
        }

        $scope.resetLastName = function () {
            $scope.isLNEdit = false;
        }
        $scope.updateLName = function (name) {

            var inputData = { Id: $scope.editformData.Id, LastName: name };
            $scope.loadingImage = true;
            var promise = userservice.editLastName(JSON.stringify(inputData));
            promise
              .success(function (data) {
                  $scope.loadingImage = false;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Last Name Updated');
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
             });

        }
        $scope.updateLastName = function () {

            var inputData = { Id: $scope.editformData.Id, LastName: $scope.editformData.NewLastName };
            $scope.loadingImage = true;
            var promise = userservice.editLastName(JSON.stringify(inputData));
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.editformData.LastName = $scope.editformData.NewLastName;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Last Name Updated');
                  $scope.resetLastName();
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
                 $scope.resetLastName();

             });

        }

        $scope.getELeditTemp = function () {

            if ($scope.isELEdit) { return 'ELedit'; }
            else
            {
                return 'ELdisplay';
            }
        }

        $scope.setELeditTemp = function () {
            $scope.editformData.NewEmail = $scope.editformData.Email
            $scope.isELEdit = true;
        }

        $scope.resetEmail = function () {
            $scope.isELEdit = false;
        }
        $scope.updateE = function (email) {

            var inputData = { Id: $scope.editformData.Id, Email: email };
            $scope.loadingImage = true;
            var promise = userservice.editEmail(JSON.stringify(inputData));
            promise
              .success(function (data) {
                  $scope.loadingImage = false;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Email updated');
              })
             .error(function (data, status, headers, config) {

                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
             });

        }
        $scope.updateEmail = function () {

            var inputData = { Id: $scope.editformData.Id, Email: $scope.editformData.NewEmail };
            $scope.loadingImage = true;
            var promise = userservice.editEmail(JSON.stringify(inputData));
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.editformData.Email = $scope.editformData.NewEmail;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Email updated');
                  $scope.resetEmail();
              })
             .error(function (data, status, headers, config) {

                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                 $scope.resetEmail();
             });

        }

        $scope.getUMeditTemp = function () {

            if ($scope.isUMEdit) { return 'UMedit'; }
            else
            {
                return 'UMdisplay';
            }
        }

        $scope.setUMeditTemp = function () {
            $scope.editformData.ManagerUserName = $scope.editformData.ManagerUserName
            $scope.isUMEdit = true;
        }

        $scope.resetUserManager = function () {
            $scope.isUMEdit = false;
        }
        $scope.refreshRoles = function (address) {
            var params = { address: address, sensor: false };

            vm.addresses = response.data.results;

        };
        //$scope.showManger = function () {
        //    var selected = $filter('filter')($scope.userList, { UserName: $scope.editformData.ManagerUserName });
        //    return ($scope.editformData.ManagerUserName && selected.length) ? selected[0].UserName : 'Not set';
        //};
        $scope.showManger = function ($item, $model) {
            console.log()
            var selected = $filter('filter')($scope.userList, { UserName: $scope.editformData.ManagerUserName });
            return ($scope.editformData.ManagerUserName && selected.length) ? selected[0].UserName : 'Not set';
        };

      
        $scope.updateUserManager = function (name) {
            var Manager = $scope.editformData.Manager;
            console.log($scope.editformData.Manager);
            //var Manager = ($filter('filter')($scope.userList, { UserName: name }))[0];
            //if (Manager == null)
            //    $.Notification.autoHideNotify('error', 'top right', 'Error', "select Manager");
            var inputData = { Id: $scope.editformData.Id, ManagerId: Manager.Id };
            $scope.loadingImage = true;
            var promise = userservice.editManager(JSON.stringify(inputData));
            promise
              .success(function (data) {
                  //console.log(Manager.UserName);
                  $scope.loadingImage = false;
                  $scope.editformData.ManagerUserName = Manager.UserName;
                  Manager.Id = 0;
                  Manager.UserName
                  //$("#userManagerSelect").val("");
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Manager updated');
                  //$scope.resetUserManager();
              })
             .error(function (data, status, headers, config) {

                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                 $scope.resetUserManager();
             });

        }

        $scope.getOverallProgress = function () {

            var inputData = { Id: $scope.editformData.Id };
            $scope.loadingImage = true;
            var promise = userservice.getOverallProgress(JSON.stringify(inputData));
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.overallProgress = data;
                  $('#overallChart').data('easyPieChart').update($scope.overallProgress);
              })
             .error(function (data, status, headers, config) {

                 $scope.loadingImage = false;
                 $scope.overallProgress = 0;
                 $('#overallChart').data('easyPieChart').update($scope.overallProgress);
             });


        }

        $scope.getAligned = function () {

            var inputData = { Id: $scope.editformData.Id };
            $scope.loadingImage = true;
            var promise = userservice.getAligned(JSON.stringify(inputData));
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.aligned = data;
                  $('#alignedChart').data('easyPieChart').update($scope.aligned);
              })
             .error(function (data, status, headers, config) {

                 $scope.loadingImage = false;
                 $scope.aligned = 0;
                 $('#alignedChart').data('easyPieChart').update($scope.aligned);
             });

        }

        $scope.previewImage = function (files) {

            if (FileReader && files && files.length) {
                var fr = new FileReader();
                fr.onload = function () {
                    var imgtag = document.getElementById("previewImageTag");
                    imgtag.src = fr.result;
                }
                fr.readAsDataURL(files[0]);
                $scope.uploadedImage = files[0];
            }

        }

        $scope.uploadImage = function () {
            var formData = new FormData();
            formData.append($scope.uploadedImage.name, $scope.uploadedImage);
            formData.append('username', $scope.editformData.Id);

            $scope.loadingImage = true;
            var promise = userservice.uploadImage(formData);

            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.editformData.Avatar = data;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Avatar updated');
                  document.getElementById("previewImageTag").src = '';
              })
             .error(function (data, status, headers, config) {

                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)

             });
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

        $scope.prepareassignees = function () {
            $scope.assigneeList = [];
            $scope.assigneeList.push({
                name: '<strong>Assignees</strong>',
                msGroup: true
            });

            if ($scope.teamList.length > 0) {

                $scope.assigneeList.push({
                    name: '<strong>Teams</strong>',
                    msGroup: true
                });

                angular.forEach($scope.teamList, function (value, key) {

                    $scope.assigneeList.push({
                        icon: '<img  src="/images/team.png" />',
                        name: value.name,
                        id: value.id,
                        email: '',
                        isteam: true,
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

        $scope.getRoleFromSelect = function () {

            angular.forEach($scope.editformData.Roles, function (value, key) {

                for (var i = 0; i < $scope.roleList.length; i++) {

                    if (value.Id == $scope.roleList[i].Id) {
                        value.Name = $scope.roleList[i].Name;
                    }
                }

            });

        }

        $scope.objectiveDeleted = function (idx) {
            //console.log('objectiveDeleted- ', $scope.editformData.Objective)
            $scope.editformData.Objective.splice(idx, 1);
        }

        $scope.Init();
    }
]);
