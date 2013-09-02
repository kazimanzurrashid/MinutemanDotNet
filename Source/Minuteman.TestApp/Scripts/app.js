/* jshint browser: true */
/* global jQuery: false */

(function ($) {

    var MAX_HISTORY = 300,
        liveFeedProxy = $.connection.liveFeedsHub,
        data = generatePlaceholderData(),
        plot;

    liveFeedProxy.client.update = function(eventName, drilldown, timestamp, count) {
        var event = {
            eventName: eventName,
            drilldown: drilldown,
            timestamp: timestamp,
            count: count
        };
        console.dir(event);
        data.push(event);
        if (data.length > MAX_HISTORY) {
            data = data.slice(1, MAX_HISTORY + 1);
        }
        updateGraph();
    };

    function getData() {
        var result = [], i, length;
        for (i = 0, length = data.length; i < length; i++) {
            result.push([i, data[i].count]);
        }
        return result;
    }

    function updateGraph() {
        plot.setData([getData()]);
        plot.draw();
    }

    function generatePlaceholderData() {
        var ds =[], i, length;
        for (i = 0, length = MAX_HISTORY; i < length; i++) {
            ds.push({ count : 0});
        }
        return ds;
    }
    
    $.connection.hub.start(function() {
        plot = $.plot("#placeholder", [getData()], {
            series: {
                shadowSize: 0
            },
            yaxis: {
                min: 0,
                max: 12
            },
            xaxis: {
                show: false
            }
        });
        plot.draw();
    });
})(jQuery);