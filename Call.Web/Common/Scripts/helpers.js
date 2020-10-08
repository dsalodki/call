var App = App || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('Call');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

})(App);