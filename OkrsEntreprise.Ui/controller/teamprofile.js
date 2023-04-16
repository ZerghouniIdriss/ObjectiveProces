okrsapp.controller('teamprofile', ['$scope', '$location', '$routeParams', 'localdata', 'teamservice',
    function ($scope, $location, $routeParams, localdata, teamservice) {

        $scope.isNMEdit = false;
        $scope.isDesEdit = false;
        $scope.addformData = {};
        $scope.newTeamUser = 0;

        $scope.Init = function () {
            $scope.getuser();
            $scope.getTeam($routeParams.team);

            $("#employeeSelect").on("change", function (e) {
                $scope.$apply(function () {

                    if (e.val == '') {

                        $scope.newTeamUser = 0;
                    }
                    else {

                        $scope.newTeamUser = parseInt(e.val);
                    }
                });

            });

            $('.chart').easyPieChart({
                barColor: '#77EEEE',
                size: 100
            });
        };

        //tab
        $scope.tab = 1;
        $scope.setTab = function (newTab) {
            $scope.tab = newTab;
        };
        $scope.isSet = function (tabNum) {
            return $scope.tab === tabNum;
        };
        //tab

        $scope.getTeam = function (id) {

            $scope.loadingImage = true;
            var promise = teamservice.retrieveTeam(id);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  console.log(data);
                  $scope.editformData = data;
                  $scope.getOverallProgress();
                  $scope.getAligned();

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
       
        $scope.deleteUser = function (index) {

            swal({
                title: 'Do you want to delete user - ' + $scope.editformData.Users[index].UserName + ' from team - ' + $scope.editformData.name,
                text: "You will have to add it again later if required",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: true
            }, function () {

                $scope.loadingImage = true;

                var inputUser = [];
                inputUser.push($scope.editformData.Users[index]);
                var inputData = { id: $scope.editformData.id, Users: inputUser };

                var promise = teamservice.deleteUser(JSON.stringify(inputData));
                promise
                  .success(function (data) {
                      $scope.editformData.Users.splice(index, 1);
                      $.Notification.autoHideNotify('success', 'top right', 'Success', 'User deleted');
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

        $scope.getOverallProgress = function () {

            var inputData = { id: $scope.editformData.id };
            $scope.loadingImage = true;
            var promise = teamservice.getOverallProgress(JSON.stringify(inputData));
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

            var inputData = { id: $scope.editformData.id };
            $scope.loadingImage = true;
            var promise = teamservice.getAligned(JSON.stringify(inputData));
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

        $scope.getNMeditTemp = function () {
            if ($scope.isNMEdit) {
                return 'NMedit';
            }
            else {
                return 'NMdisplay';
            }
        }

        $scope.setNMeditTemp = function () {
            $scope.editformData.NewNM = $scope.editformData.name;
            $scope.isNMEdit = true;
        }

        $scope.resetNM = function () {
            $scope.isNMEdit = false;
        }

        $scope.updateNM = function (name) {
            $scope.editformData.NewNM = name;
            var inputData = { id: $scope.editformData.id, name: $scope.editformData.NewNM };
            $scope.loadingImage = true;
            var promise = teamservice.editNM(JSON.stringify(inputData));
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.editformData.name = $scope.editformData.NewNM;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Name updated');
                  $scope.resetNM();
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
                 $scope.resetNM();
             });

        }

        $scope.getDeseditTemp = function () {

            if ($scope.isDesEdit) { return 'Desedit'; }
            else
            {
                return 'Desdisplay';
            }
        }

        $scope.setDeseditTemp = function () {
            $scope.editformData.NewDes = $scope.editformData.description;
            $scope.isDesEdit = true;
        }

        $scope.resetDes = function () {
            $scope.isDesEdit = false;
        }

        $scope.updateDes = function (des) {
            $scope.editformData.NewDes = des;
            var inputData = { id: $scope.editformData.id, description: $scope.editformData.NewDes };
            $scope.loadingImage = true;
            var promise = teamservice.editDes(JSON.stringify(inputData));
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.editformData.description = $scope.editformData.NewDes;
                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Description updated');
                  $scope.resetDes();
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
                 $scope.resetDes();

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
            formData.append('teamname', $scope.editformData.id);

            $scope.loadingImage = true;
            var promise = teamservice.uploadImage(formData);

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

        $scope.getuser = function () {

            $scope.loadingImage = true;
            var promise = teamservice.searchuser($scope.addformData);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  //console.log(data);
                  $scope.userList = data;
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

        $scope.addUser = function () {

            var found = $scope.userExists($scope.newTeamUser);

            if (!found) {

                if ($scope.newTeamUser != 0 && $scope.newTeamUser != '') {

                    var inputUser = [];
                    var usr = { Id: $scope.newTeamUser };
                    inputUser.push(usr);

                    var inputData = { id: $scope.editformData.id, Users: inputUser };

                    var promise = teamservice.addUser(JSON.stringify(inputData));
                    promise
                      .success(function (data) {

                          angular.forEach($scope.userList, function (value, key) {
                              if (value.Id == $scope.newTeamUser)
                                  $scope.editformData.Users.push(value);
                          });

                          $scope.newTeamUser = 0;
                          //employeeSelect.val('').trigger("change");
                          $.Notification.autoHideNotify('success', 'top right', 'Success', 'User added');
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
                    $scope.ErrorMessage = 'Select user to assign to team';
                    $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage)
                }
            }
            else {
                $scope.ErrorMessage = 'User already found in team';
                $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
            }
        }

        $scope.userExists = function (userId) {

            var found = false;

            angular.forEach($scope.editformData.Users, function (value, key) {

                if (value.Id == userId) {
                    found = true;
                    return found;
                }

            });

            return found;
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

        $scope.Init();
    }
]);
