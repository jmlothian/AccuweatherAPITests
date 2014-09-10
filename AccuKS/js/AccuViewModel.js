function AccuViewModel() {
    var self = this;
    self.searchResults = ko.observableArray(new Array());
    self.forecast12Hours = ko.observableArray(new Array());
    self.currentWeather = ko.observable(null);
    self.currentCity = ko.observable(null);
    self.currentWeatherImage = ko.computed(function () {
        if (self.currentWeather() != null) {
            return self.getForecastIcon(self.currentWeather());
        }
        return "";
    });

    self.displaySearch = ko.computed(function () {
        return self.searchResults().length == 0 ? true : false;
    });


    self.search = function () {
        self.currentWeather(null);
        self.currentCity(null);
        self.forecast12Hours([]);
        $.getJSON("./api/Location/GetSearch?q=" + $("#txtSearch").val())
            .done(function (data) {
                //optionally, we'd create a custom object as a model and load these values into it
                self.searchResults($.parseJSON(data));
            })
            .fail(function (jqxhr, textStatus, error) {
                alert("No entry was found for this search.  Please revise and try again.");
            });
    }

    self.selectCity = function (item) {
        if (item != null) {
            self.currentCity(item);
            $.getJSON("./api/Weather/GetCurrent/" + self.currentCity().Key)
                .done(function (data) {
                    self.currentWeather($.parseJSON(data)[0]);
                });
            $.getJSON("./api/Weather/GetForecast/" + self.currentCity().Key)
                .done(function (data) {
                    self.forecast12Hours($.parseJSON(data));
                });
        }
    }

    self.getForecastIcon = function (data) {
        var ico = self.getIcon(data.WeatherIcon);
        return "http://apidev.accuweather.com/developers/Media/Default/WeatherIcons/" + ico + "-s.png";
    }

    self.getIcon = function (val) {
        var val = val + ""; //convert to string
        if (val.length < 2)
            val = "0" + val;
        return val;
    }

    self.getDateTime = function (val) {
        var d = new Date(0);
        d.setUTCSeconds(val.EpochDateTime);
        var hours = d.getHours();
        var minutes = d.getMinutes();
        minutes = minutes < 10 ? "0" + minutes : minutes;
        var seconds = d.getSeconds();
        seconds = seconds < 10 ? "0" + seconds : seconds;
        var postFix = "AM";
        if (hours > 12) {
            hours = hours - 12;
            postFix = "PM";
        }
        if (hours == 0)
            hours = 12;
        return hours + ":" + minutes + ":" + seconds + " " + postFix;
    }
}

function Init() {
    ko.applyBindings(new AccuViewModel());
}
