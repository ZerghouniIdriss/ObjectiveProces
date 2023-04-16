okrsapp.factory('managepermission', function ($rootScope, localdata) {

    //here we have mapped to role ids from database
    //definedroutes is main object for role management. it can come from database or some config file or hard code as below
    //Employee - 1
    //Manager  - 2
    //Admin    - 3
    //SysAdmin - 4

    var definedroutes = [
        //users
        { name: '/login', permitted: [] },
        { name: '/register', permitted: [] },
        { name: '/recoverpassword', permitted: [] },

        //Employee
        { name: '/sidedashboard', permitted: [1, 2, 3, 4] },
        { name: '/topdashboard', permitted: [1, 2, 3, 4] },
        { name: '/topsearch', permitted: [1, 2, 3, 4] },
        { name: '/topnotification', permitted: [4] },
        { name: '/topavatarimage', permitted: [1, 2, 3, 4] },
        { name: '/navigatetonewgoal', permitted: [1, 2, 3, 4] },

         
        { name: '/objectives', permitted: [1, 2, 3, 4] },
        { name: '/map', permitted: [1, 2, 3, 4] },
        { name: '/dashboard', permitted: [1, 2, 3, 4] },
        { name: '/teamdashboard', permitted: [1, 2, 3, 4] },
        { name: '/companydashboard', permitted: [1, 2, 3, 4] },
        { name: '/goal', permitted: [1, 2, 3, 4] },
        { name: '/editgoal', permitted: [1, 2, 3, 4] },
        { name: '/users', permitted: [1, 2, 3, 4] },
        { name: '/user', permitted: [1, 2, 3, 4] },
        { name: '/edituser', permitted: [1, 2, 3, 4] },
        { name: '/teams', permitted: [1, 2, 3, 4] },
        { name: '/team', permitted: [1, 2, 3, 4] },
        { name: '/editteam', permitted: [1, 2, 3, 4] },
        { name: '/activities', permitted: [1, 2, 3, 4] },
        { name: '/profile', permitted: [1, 2, 3, 4] },
        { name: '/closedobjectives', permitted: [1, 2, 3, 4] },


        //Admin
        { name: '/categories', permitted: [4] },
        { name: '/category', permitted: [4] },
        { name: '/editcategory', permitted: [4] },
        { name: '/status', permitted: [4] },
        { name: '/newstatus', permitted: [4] },
        { name: '/editstatus', permitted: [4] },

        //SysAdmin
        { name: '/indicator', permitted: [4] },
        { name: '/editindicator', permitted: [4] },
        { name: '/indicators', permitted: [4] },

        { name: '/sessions', permitted: [4] },
        { name: '/oneOnOneSession', permitted: [4] },
        { name: '/sessiondetail', permitted: [4] },
        { name: '/employeesessions', permitted: [4] },
        { name: '/employeesession', permitted: [4] },
        { name: '/employeesessiondetail', permitted: [4] },
        { name: '/objectiveFollowedBy', permitted: [4] },
        { name: '/surveys', permitted: [4] },
        { name: '/editdomain', permitted: [4] },
        { name: '/masters', permitted: [4] },
        { name: '/tenants', permitted: [4] },
        { name: '/recognition', permitted: [4] },
        
        { name: '/edittenant', permitted: [4] },
        { name: '/tenant', permitted: [4] },
        { name: '/roles', permitted: [4,3] },
        { name: '/invitecoworkers', permitted: [4] },
        { name: '/inviteusers', permitted: [4] }
       
    ];

    var checkRoles = function (permitted) {

        var un = localdata.getuserData().un;
        if (un == undefined) {
            return true; //if user is not logged in
        }

        var userRoleList = un.userRoles.split(",");

        for (var i = 0; i < userRoleList.length; i++) {
            var value = userRoleList[i];

            if (permitted.indexOf(parseInt(value)) != -1) {
                return true;
            }
        };

        return false;
    };

    return {

        isPermitted: function (route) {

            for (var i = 0; i < definedroutes.length; i++) {

                var value = definedroutes[i];
                if (route.startsWith(value.name)) {
                    if (value.permitted.length == 0 || checkRoles(value.permitted)) {
                        return true;
                        break;
                    }
                }
            }

            return false;
        }
    }

});