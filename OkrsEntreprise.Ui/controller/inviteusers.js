okrsapp.controller('inviteusers', ['$scope', '$location', 'localdata', 'managepermission', 'userservice', function ($scope, $location, localdata, managepermission, userservice) {

    $scope.userDomain = '';
    $scope.defaultInvite = 5;
    $scope.invitationList = [];
    $scope.editdomain = false;

    $scope.Init = function () {
        $scope.initializeList();

        $scope.userDomain = $scope.getDomain();
        $scope.$on('userDataChanged', $scope.UserNameChanged);

        if (managepermission.isPermitted('/editdomain')) {
            $scope.editdomain = true;
        }
    }

    $scope.initializeList = function () {
        $scope.invitationList = [];

        for (var i = 0; i < $scope.defaultInvite; i++) {
            $scope.invitationList.push({ no: i + 1, email: '', userDomain: $scope.getDomain() });
        }
    }

    $scope.UserNameChanged = function () {
        $scope.userDomain = $scope.getDomain();
    }

    $scope.getDomain = function () {
        var userEmail = localdata.getuserData().un.userEmail;
        return  userEmail.split("@")[1];
    }

    $scope.inviteMore = function () {
        $scope.invitationList.push({ no: $scope.invitationList.length + 1, email: '' });
    }

    $scope.sendInvitation = function () {

        var inviteList = [];

        for (var i = 0; i < $scope.invitationList.length; i++) {

            if ($scope.invitationList[i].email != '') {
                inviteList.push($scope.invitationList[i].email + '@' + $scope.invitationList[i].userDomain);
            }
        }

        if (inviteList.length == 0) {
            $.Notification.autoHideNotify('error', 'top right', 'Error', 'Add atleast one user to invite')
        }
        else {
            var toemail = { userId: localdata.getuserData().un.userId, emails: inviteList };

            var promise = userservice.inviteOthers(toemail);
            promise
              .success(function (data) {

                  $scope.loadingImage = false;

                  $.Notification.autoHideNotify('success', 'top right', 'Success', 'Users invited')
                  $scope.initializeList();

              })
             .error(function (data, status, headers, config) {
                 if (status == 400) {
                     $scope.ErrorMessage = $scope.parseErrors(data);
                 }
                 else if (status == 404) {

                     $scope.ErrorMessage = 'Requested link not found';
                 }
                 else {
                     $scope.ErrorMessage = 'Sorry failed to send mail!';
                 }

                 $scope.loadingImage = false;
                 $.Notification.autoHideNotify('error', 'top right', 'Error', $scope.ErrorMessage);
             });


        }

    }


    $scope.parseErrors = function(response) {
        var errors = [];
        for (var key in response.ModelState) {
            for (var i = 0; i < response.ModelState[key].length; i++) {
                errors.push(response.ModelState[key][i]);
            }
        }
    }


    $scope.Init();
}]);