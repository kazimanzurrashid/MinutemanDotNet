/* jshint browser: true */
/* global jQuery: false, _: false, Backbone: false, moment: false */

(function ($, _, Backbone, moment) {
    'use strict';

    var App = {};

    function clientUrl() {
        var path = _(arguments).toArray().join('/');

        if (path.length && path.indexOf('/') === 0) {
            path = path.substring(1);
        }

        return '#!/' + path;
    }

    App.Router = Backbone.Router.extend({
        routes: {
            '!/event-activity/:name/:timeframe': 'showEventActivity',
            '!/event-activity/:name': 'noTimeframeSelectedForEventActivity',
            '!/event-activity': 'showNoEventSelected',
            '!/user-activity/:name/:timeframe': 'showUserActivity',
            '!/user-activity/:name': 'noTimeframeSelectedForUserActivity',
            '!/user-activity': 'showNoEventSelected',
            '': 'showNoEventSelected'
        },

        showEventActivity: function (name, timeframe) {
            this.removeCurrentView();

            var targetElement = this.contentElement.empty();

            App.eventActivityProxy.server.timeframes(name).done(_(function (timeframes) {
                var timeframeList = new App.TimeframeList({
                    urlPrefix: 'event-activity/' + name,
                    timeframes: timeframes,
                    selectedTimeframe: timeframe
                });
                var graph = new App.EventActivityGraph({
                    eventName: name,
                    timeframe: timeframe
                });
                targetElement.append(
                    timeframeList.$el,
                    graph.$el);

                timeframeList.render();
                graph.render();

                this.currentView = graph;

            }).bind(this));
        },

        showUserActivity: function(name, timeframe) {
            this.removeCurrentView();
        },

        noTimeframeSelectedForEventActivity: function (name) {
            this.showNoTimeframeSelected(
                'event-activity',
                name,
                App.eventActivityProxy,
                App.eventMenuList,
                App.userMenuList);
        },

        noTimeframeSelectedForUserActivity: function (name) {
            this.showNoTimeframeSelected(
                'user-activity',
                name,
                App.userActivityProxy,
                App.userMenuList,
                App.eventMenuList);
        },

        showNoEventSelected: function() {
            this.removeCurrentView();

            var targetElement = this.contentElement.empty(),
                messagebox = new App.MessageBox({
                    className: 'alert-box alert',
                    message: 'Select an event from the left menu.'
                });
            targetElement.append(messagebox.render().$el);
        },

        showNoTimeframeSelected: function (
            prefix,
            name,
            proxy,
            currentView,
            otherView) {

            this.removeCurrentView();

            var targetElement = this.contentElement.empty();

            proxy.server.timeframes(name)
                .done(function (timeframes) {
                    var timeframeList = new App.TimeframeList({
                        urlPrefix: prefix + '/' + name,
                        timeframes: timeframes
                    }),
                        messagebox = new App.MessageBox({
                            className: 'alert-box alert',
                            message: 'Select a timeframe from the the above.'
                        });

                    targetElement.append(
                        timeframeList.render().$el,
                        messagebox.render().$el);

                    otherView.deselectAll();
                    currentView.deselectAll().select(name);
                });
        },

        initialize: function () {
            this.currentView = null;
            this.contentElement = $('#content');
        },

        removeCurrentView : function() {
            if (this.currentView) {
                this.currentView.remove();
                this.currentView = void(0);
            }
        }
    });

    App.MenuList = Backbone.View.extend({
        itemTemplate: _('<li><a href="<%= url %>"><%= text %></a></li>').template(),
        emptyItem: '<li>no event exists.</li>',

        setEvents: function(events) {
            this.events = events;
            return this;
        },

        render: function () {
            var prefix = this.options.prefix,
                template = this.itemTemplate,
                html = '';

            if (this.events.length) {
                _(this.events).each(function(event) {
                    html += template({
                        url: clientUrl(prefix, event),
                        text: event
                    });
                });
            } else {
                html = this.emptyItem;
            }

            this.el.innerHTML = html;

            return this;
        },

        select: function(event) {
            var active = this.$el.find('li').find('a:contains("' + event + '")');
            active.parent('li').addClass('active');
            return this;
        },

        deselectAll: function() {
            this.$el.find('li.active').removeClass('active');
            return this;
        }
    });

    App.TimeframeList = Backbone.View.extend({
        tagName: 'dl',
        className: 'sub-nav',
        template: _('<dd class="<%= (current) ? \"active\" : \"\" %>"><a href="<%= url %>"><%= text %></a></dd>').template(),

        render: function () {
            var template = this.template,
                label = this.options.label || 'Timeframe:',
                selectedTimeframe = (this.options.selectedTimeframe || '').toLowerCase(),
                urlPrefix = this.options.urlPrefix,
                html = '<dt>' + label + '</dt>';

            _(this.options.timeframes).each(function (timeframe) {
                var current = selectedTimeframe === timeframe.toLowerCase();

                html += template({
                    text: timeframe,
                    url: clientUrl(urlPrefix, timeframe.toLowerCase()),
                    current: current
                });
            });

            this.el.innerHTML = html;

            return this;
        }
    });

    App.MessageBox = Backbone.View.extend({
        render: function() {
            this.el.innerHTML = this.options.message;
            return this;
        }
    });

    App.EventActivityGraph = Backbone.View.extend({
        maxHistory: 7,
        className: 'event-activity-graph',

        initialize: function () {
            App.eventActivityProxy.server.subscribe(this.options.eventName);
            this.listenTo(App.pubsub, 'eventActivity:received', this.onDataReceive);
        },

        render: function () {
            this.data = {};

            this.chart = this.$el.plot([this.processData()], {
                series: {
                    shadowSize: 0,
                    lines: {
                        fill: true,
                        show: true
                    }
                },
                xaxis: {
                    mode: 'time',
                    timeformat: this.getFormat()
                },
                yaxis: {
                    min: 0
                }
            }).data('plot');

            this.chart.draw();

            return this;
        },

        remove: function() {
            App.eventActivityProxy.server.unsubscribe(this.options.eventName);
            Backbone.View.prototype.remove.call(this);

            return this;
        },

        update: function () {
            var data = this.processData();
            this.chart.setData([data]);
            this.chart.setupGrid();
            this.chart.draw();
        },

        processData: function() {
            var data = this.data,
                result = [],
                keys = _(data).keys().sort(function (x, y) {
                    return parseInt(x, 10) - parseInt(y, 10);
                }),
                i,
                length,
                key;

            for (i = 0, length = keys.length; i < length; i++) {
                key = keys[i];
                result.push([parseInt(key, 10), data[key]]);
            }

            return result;
        },

        getFormat: function() {
            var timeframe = this.options.timeframe.toLowerCase();

            switch (timeframe) {
                case 'second':
                    return '%I:%M:%S%P';
                case 'minute':
                    return '%I:%M%P';
                case 'hour':
                    return '%I%P';
                case 'day':
                    return '%a';
                case 'month':
                    return '%b';
                case 'year':
                    return '%Y';
                default:
                    throw new Error('Unknown timeframe!');
            }
        },

        onDataReceive: function (info) {
            var item, latestKeys;

            if (this.options.eventName === info.eventName &&
                this.options.timeframe.toLowerCase() === info.timeframe.toLowerCase()) {

                item = {
                    timestamp: moment(info.timestamp).toDate().getTime(),
                    count: info.count
                };

                if (!_(this.data).has(item.timestamp)) {
                    this.data[item.timestamp] = 0;
                }

                this.data[item.timestamp] += item.count;

                if (_(this.data).keys().length > this.maxHistory) {
                    latestKeys = _(this.data).keys().sort(function(x, y) {
                        return parseInt(x, 10) - parseInt(y, 10);
                    });
                    this.data = _(this.data).pick(_(latestKeys).rest());
                }

                this.update();
            }
        }
    });

    App.pubsub = _({}).extend(Backbone.Events);

    App.eventActivityProxy = $.connection.eventActivityHub;

    App.eventActivityProxy.client.update = function (json) {
        var info = $.parseJSON(json);
        App.pubsub.trigger('eventActivity:received', info);
    };

    App.userActivityProxy = $.connection.userActivityHub;

    App.userActivityProxy.client.update = function (eventName, timeframe, timestamp, users) {
        var event = {
            eventName: eventName,
            timeframe: timeframe,
            timestamp: timestamp,
            users: users
        };

        App.pubsub.trigger('userActivity:received', event);
    };

    App.userMenuList = new App.MenuList({
        el: '#user-activity-list',
        prefix: 'user-activity'
    });

    App.eventMenuList = new App.MenuList({
        el: '#event-activity-list',
        prefix: 'event-activity'
    });

    App.router = new App.Router();

    $.connection.hub.start().done(function() {
        App.userActivityProxy.server.eventNames().done(function (events) {
            App.userMenuList.setEvents(events).render();
        });

        App.eventActivityProxy.server.eventNames().done(function(events) {
            App.eventMenuList.setEvents(events).render();
        });

        Backbone.history.start();
    });

    $(function() {
        $(document).foundation();
    });

    window.App = App;

})(jQuery, _, Backbone, moment);