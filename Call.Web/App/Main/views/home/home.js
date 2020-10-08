(function () {
    var chatHub = $.connection.requestHub; //get a reference to the hub

    var self = {};
    var controllerId = 'app.views.home';
    angular.module('app').controller(controllerId, [
        '$scope', function ($scope) {
            var vm = this;

            $scope.mark = function ($event, id, state) {
                for (var i = 0; i < $scope.requests.length; i++) {
                    if ($scope.requests[i].id === id) {
                        if (state === 2) {
                            abp.notify.info("request answered and deleted", $scope.requests[i].phone);
                            $scope.requests.splice(i, 1);
                        } else {
                            $scope.requests[i].state = state;
                            var elem = $($event.currentTarget);
                            elem.hide();
                            elem.next().show();
                            abp.notify.warn("request state changed to Treated", $scope.requests[i].phone);
                        }
                    }
                }

                chatHub.server.mark(id, state);
            }

            $scope.addRequest = function (request) {
                $scope.requests.push(request);
            }

            $scope.requests = self ? self.requests : null;
        }
    ]);

    chatHub.client.setTreatedTime = function (id, time) {
        var scope = angular.element($("#vm")).scope();
        if (!scope) {
            return;
        }
        scope.$apply(function () {
            for (var i = 0; i < scope.requests.length; i++) {
                if (scope.requests[i].id === id) {
                    //scope.requests.state = state;
                    scope.requests[i].treatedTime = time;
                    break;
                }
            }
        });
    }

    chatHub.client.updateState = function (id, state) {
        var i;
        var scope = angular.element($("#vm")).scope();
        if (!scope) {
            return;
        }

        switch (state) {
            case 1://Treated
                scope.$apply(function () {
                    for (i = 0; i < scope.requests.length; i++) {
                        if (scope.requests[i].id === id) {
                            //scope.requests.state = state;
                            abp.notify.warn("request treated changed and deleted", scope.requests[i].phone);
                            scope.requests.splice(i, 1);
                            break;
                        }
                    }
                });
                break;
            case 2://Answered
                scope.$apply(function () {
                    for (i = 0; i < scope.requests.length; i++) {
                        if (scope.requests[i].id === id) {
                            abp.notify.info("request answered and deleted", scope.requests[i].phone);
                            scope.requests.splice(i, 1);
                            break;
                        }
                    }
                });
                break;
        }
    }

    chatHub.client.notifyAboutRequest = function (request) { //register for incoming messages
        var showNotification = function (phone) {
            var audio = new Audio("Content/mp3/notification_sound.mp3");
            audio.play();
            var options = {
                body: '...',
                requireInteraction: true
            }
            var notification = new Notification("new request from " + phone, options);
        }

        var scope = angular.element($("#vm")).scope();
        if (scope) {
            scope.$apply(function () {
                scope.addRequest(request);
            });
        }
        abp.notify.success("new request", request.phone);
        if (!("Notification" in window)) {
            alert("This browser does not support desktop notification");
        }
        else if (Notification.permission === "granted") {
            // Если разрешено то создаем уведомлений
            showNotification(request.phone);
        }
        else if (Notification.permission !== 'denied') {
            Notification.requestPermission(function (permission) {
                // Если пользователь разрешил, то создаем уведомление 
                if (permission === "granted") {
                    showNotification(request.phone);
                }
            });
        }
    };



    chatHub.client.addNotAsweredRequests = function (requests/*, clientTenantId*/) {
        self.requests = requests;

        var scope = angular.element($("#vm")).scope();
        if (scope) {
            scope.$apply(function () {
                scope.requests = requests;
            });
        }
    }
})();