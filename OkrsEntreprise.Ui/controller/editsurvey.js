okrsapp.controller('editsurvey', ['$scope', '$location', '$routeParams', 'localdata', 'surveyservice', function ($scope, $location, $routeParams, localdata, surveyservice) {

    $scope.editformData = {};
    $scope.goalcatList = [];
    $scope.goalstatusList = [];
    $scope.editname = false;
    $scope.edthdr = 'h';
    $scope.editdes = false;
    $scope.oldtitle = '';
    $scope.olddes = '';
    $scope.oldcategory = {};
    $scope.oldstatus = {};
    $scope.oldisprivate = false;
    $scope.format = 'MM/dd/yyyy';
    $scope.ErrorMessage = '';
    $scope.goalList = [];

    $scope.Init = function () {
    }

    $scope.addnewgoal = function () {
        $location.path('/editgoal/' + $routeParams.goal + '/goal');
    }

    $scope.deletegoal = function () {
        if (confirm("Do you want to delete the goal?")) {

            var promise = dataservice.deletegoal($routeParams.goal);
            promise
              .success(function (data) {
                  alert('Objective deleted');
                  $location.path('objectives');
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Not Found';
                 }
             });

        }
    }

    $scope.deleteparentgoal = function () {

        if (confirm('Do you want to delete parent objective - ' + $scope.editformData.parent.title)) {
            $scope.loadingImage = true;
            var oldparent = $scope.editformData.parent;
            $scope.editformData.parent = null;

            var promise = dataservice.editparent($scope.editformData);
            promise
              .success(function (data) {
                  alert('Parent Objective deleted');
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Not Found';
                 }
                 $scope.editformData.parent = oldparent;
             });
        }
    }

    $scope.deletechildgoal = function (idx) {

        if (confirm('Do you want to delete child objective - ' + $scope.editformData.children[idx].title)) {

            $scope.loadingImage = true;

            var promise = dataservice.deletesubgoal($scope.editformData.children[idx].parentid, $scope.editformData.children[idx].id);
            promise
              .success(function (data) {

                  $scope.editformData.children.splice(idx, 1);
                  alert('Sub Objective deleted');

              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.ErrorMessage = 'Not Found';
                 }
             });
        }
    }

    $scope.addchildgoal = function () {

        var promise = dataservice.addsubgoal($scope.editformData.id, childselect.val());
        promise
          .success(function (data) {

              for (var i = 0; i < $scope.goalList.length; i++) {

                  if ($scope.goalList[i].id == childselect.val()) {
                      var child = { "parentid": parseInt($scope.editformData.id), "id": parseInt($scope.goalList[i].id), "title": $scope.goalList[i].title };
                      $scope.editformData.children.push(child);
                  }
              }

              alert('child added');
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {

                 $scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.ErrorMessage = 'Not Found';
             }

         });

    }

    $scope.getgoals = function () {

        $scope.loadingImage = true;
        var promise = dataservice.searchgoal($scope.editformData);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.goalList = data;


          })
         .error(function (data, status, headers, config) {
             if (status = 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
         });

    }

    $scope.getgoal = function (id) {
        $scope.loadingImage = true;
        var promise = dataservice.retrievgoal(id);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.editformData = data;
              $scope.checkassign();
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $location.path('/objectives');
             }
         });

    }

    $scope.gettitletemp = function () {
        if ($scope.editname) return 'edittitle'; else return 'distitle';
    }

    $scope.edittitle = function () {
        $scope.oldtitle = $scope.editformData.title;
        $scope.editname = true;
    }

    $scope.updatetitle = function () {

        $scope.loadingImage = true;
        var promise = dataservice.editgoaltitle($scope.editformData.id, $scope.editformData.title);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.oldtitle = '';
              $scope.editname = false;
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
                 $scope.resettitle();
             }
         });

    }

    $scope.resettitle = function () {
        $scope.editformData.title = $scope.oldtitle;
        $scope.oldtitle = '';
        $scope.editname = false;
    }

    $scope.getheadertemp = function () {
        if ($scope.edthdr == 'h') return 'dishdr';
        else if ($scope.edthdr == 's') return 'edtstus';
        else if ($scope.edthdr == 'c') return 'edtcat';
        else if ($scope.edthdr == 'p') return 'edtpvt';
        else if ($scope.edthdr == 'd') return 'edtdd';
    }

    $scope.editstatus = function () {
        $scope.oldstatus = $scope.editformData.status;

        for (var i = 0; i < $scope.goalstatusList.length; i++) {

            if ($scope.goalstatusList[i].id == $scope.oldstatus.id) {
                $scope.editformData.status = $scope.goalstatusList[i];
            }
        }

        $scope.edthdr = 's';
    }

    $scope.updatestus = function () {

        $scope.loadingImage = true;
        var promise = dataservice.editgoalstatus($scope.editformData.id, $scope.editformData.status);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.oldstatus = {};
              $scope.edthdr = 'h';
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
                 $scope.resetstus();
             }
         });

    }

    $scope.resetstus = function () {
        $scope.editformData.status = $scope.oldstatus;
        $scope.oldstatus = {};
        $scope.edthdr = 'h';
    }

    $scope.editcategory = function () {
        $scope.oldcategory = $scope.editformData.category;

        for (var i = 0; i < $scope.goalcatList.length; i++) {

            if ($scope.goalcatList[i].id == $scope.oldcategory.id) {
                $scope.editformData.category = $scope.goalcatList[i];
            }
        }

        $scope.edthdr = 'c';
    }

    $scope.updatecat = function () {

        $scope.loadingImage = true;
        var promise = dataservice.editgoalcategory($scope.editformData.id, $scope.editformData.category);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.oldcategory = {};
              $scope.edthdr = 'h';
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
                 $scope.resetcat();
             }
         });

    }

    $scope.resetcat = function () {
        $scope.editformData.category = $scope.oldcategory;
        $scope.oldcategory = {};
        $scope.edthdr = 'h';
    }

    $scope.editisprivate = function () {
        $scope.oldisprivate = $scope.editformData.isprivate;
        $scope.edthdr = 'p';
    }

    $scope.updatepvt = function () {

        $scope.loadingImage = true;
        var promise = dataservice.editgoalisprivate($scope.editformData.id, $scope.editformData.isprivate);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.oldisprivate = false;
              $scope.edthdr = 'h';
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
                 $scope.resetpvt();
             }
         });

    }

    $scope.resetpvt = function () {
        $scope.editformData.isprivate = $scope.oldisprivate;
        $scope.oldisprivate = false;
        $scope.edthdr = 'h';
    }

    $scope.editduedate = function () {
        $scope.oldduedate = $scope.editformData.duedate;
        $scope.edthdr = 'd';
    }

    $scope.updateduedate = function () {

        $scope.loadingImage = true;
        var promise = dataservice.editgoalduedate($scope.editformData.id, $scope.editformData.duedate);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.oldduedate = false;
              $scope.edthdr = 'h';
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
                 $scope.resetduedate();
             }
         });

    }

    $scope.resetduedate = function () {
        $scope.editformData.duedate = $scope.oldduedate;
        $scope.oldduedate = '';
        $scope.edthdr = 'h';
    }

    $scope.getdestemp = function () {
        if ($scope.editdes) return 'edtdes'; else return 'disdes';
    }

    $scope.editdest = function () {
        $scope.olddes = $scope.editformData.description;
        $scope.editdes = true;
    }

    $scope.updatedes = function () {

        $scope.loadingImage = true;
        var promise = dataservice.editgoaldes($scope.editformData.id, $scope.editformData.description);
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.olddes = '';
              $scope.editdes = false;
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
                 $scope.resetdes();
             }
         });

    }

    $scope.resetdes = function () {
        $scope.editformData.description = $scope.olddes;
        $scope.olddes = '';
        $scope.editdes = false;
    }

    $scope.getgoalcat = function () {

        $scope.loadingImage = true;
        var promise = dataservice.searchgoalcat();
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.goalcatList = data;

          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
         });

    }

    $scope.getgoalstatus = function () {

        $scope.loadingImage = true;
        var promise = dataservice.searchgoalstatus();
        promise
          .success(function (data) {

              $scope.loadingImage = false;
              $scope.goalstatusList = data;
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 //console.log(data);
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
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

    $scope.currentUser = function (item) {
        return localdata.getuserName().un == item.user.UName;
    }

    $scope.addcomment = function () {
        if ($scope.NewUserComment != undefined && $scope.NewUserComment != '') {

            $scope.loadingImage = true;
            var promise = dataservice.addcomment($scope.editformData.id, $scope.NewUserComment);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  //var usercomment = { "id": null, "text": $scope.NewUserComment, "user": { "id": null, "FName": null, "LName": null, "UName": localdata.getuserName().un, "Email": null, "Password": null, "ConfirmPassword": null } };
                  $scope.editformData.comments.push(data);
                  $scope.NewUserComment = '';
                  //console.log(data);
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
             });

        }
    }

    $scope.selcom = {};

    $scope.editcomment = function (com) {
        $scope.selcom = angular.copy(com);
    }

    $scope.resetcomment = function () {
        $scope.selcom = {};
    }

    $scope.deletecomment = function (indx) {
        if (confirm('Do you want delete comment - ' + $scope.editformData.comments[indx].text)) {

            var promise = dataservice.deletecomment($scope.editformData.comments[indx]);
            promise
              .success(function () {

                  $scope.loadingImage = false;
                  $scope.editformData.comments.splice(indx, 1);
                  alert('Comment deleted.');
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Not Found';
                 }

             });
        }
    }

    $scope.updatecomment = function (indx) {

        var promise = dataservice.editcomment($scope.editformData.comments[indx], $scope.selcom.text);
        promise
          .success(function () {

              $scope.loadingImage = false;
              $scope.editformData.comments[indx].text = $scope.selcom.text;
              $scope.resetcomment();
          })
         .error(function (data, status, headers, config) {
             if (status == 400) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = $scope.parseErrors(data);
             }
             else if (status == 404) {
                 $scope.loadingImage = false;
                 $scope.ErrorMessage = 'Not Found';
             }

         });

    }

    $scope.getcomtemp = function (com) {
        if ($scope.selcom.id == com.id) return 'editcomment'; else return 'discomment';
    }

    $scope.selkr = {};
    $scope.showKeymsg = false;
    $scope.keyresulttitle = '';

    $scope.addkeyresult = function () {
        if ($scope.keyresulttitle == undefined || $scope.keyresulttitle.length == 0) {
            $scope.showKeymsg = true;
        }
        else {
            $scope.showKeymsg = false;

            var promise = dataservice.addkeyresult($scope.editformData.id, $scope.keyresulttitle);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;

                  $scope.editformData.keyresults.push(data);
                  $scope.keyresulttitle = '';
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Not Found';

                 }

             });

        }
    }

    $scope.getkrtemp = function (kr) {

        if (kr.id === $scope.selkr.id) return 'kredit';
        else return 'krdis';
    };

    $scope.editkr = function (kr) {
        $scope.selkr = angular.copy(kr);
    };

    $scope.resetkr = function () {
        $scope.selkr = {};
    };

    $scope.updatekr = function (idx) {

        if ($scope.selkr.title == undefined || $scope.selkr.title.length == 0) {
            $scope.showKeymsg = true;
        }
        else {

            $scope.showKeymsg = false;

            var promise = dataservice.editkeyresult($scope.selkr);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;
                  $scope.editformData.keyresults[idx].title = $scope.selkr.title;
                  $scope.resetkr();
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Not Found';

                 }

             });
        }
    }

    $scope.deletekr = function (idx) {

        if (confirm('Do you want delete keyresult - ' + $scope.editformData.keyresults[idx].title)) {

            var promise = dataservice.deletekeyresult($scope.editformData.keyresults[idx]);
            promise
              .success(function () {

                  $scope.loadingImage = false;
                  $scope.editformData.keyresults.splice(idx, 1);
                  alert('KeyResult deleted.');
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Not Found';
                 }

             });
        }

    }

    $scope.isassign = false;

    $scope.checkassign = function () {

        $scope.isassign = false;

        for (var i = 0; i < $scope.editformData.assignees.length; i++) {

            if ($scope.editformData.assignees[i].name == localdata.getuserName().un) {
                $scope.isassign = true;
                break;
            }
        }
    }

    $scope.assigngoal = function () {

        var promise = null;

        if ($scope.isassign) {
            promise = dataservice.removeme($scope.editformData);

            promise
              .success(function (data) {

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
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Not Found';

                 }

             });

        }
        else {
            promise = dataservice.assignme($scope.editformData);

            promise
              .success(function (data) {
                  $scope.loadingImage = false;
                  var usr = { "id": 0, "name": localdata.getuserName().un, "isteam": false }
                  $scope.editformData.assignees.push(usr);
                  $scope.checkassign();
              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.loadingImage = false;
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {

                     $scope.loadingImage = false;
                     $scope.ErrorMessage = 'Not Found';

                 }

             });
        }


    }


    $scope.Init();

}]);