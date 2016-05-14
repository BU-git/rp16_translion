(function() {
    'use strict';

    function initCache(valueCache, allInputs) {
        allInputs.each(function () {
            valueCache.push($(this).val());
        });
    };

    function checkCache(valueCache, allInputs, submit, e) {
        var submitAllowed = false;
        allInputs.each(function (index, val) {
            var curVal = $(this).val();

            if (valueCache[index] !== curVal) {
                valueCache[index] = curVal;

                if (!submitAllowed)
                    submitAllowed = true;
            }
        });

        if (!submitAllowed) {
            submit.attr('disabled', 'disabled');
            e.preventDefault();
        }
        else
            submit.removeAttr('disabled');
    };

    $(document).ready(function() {
        var form = $('form');
        var submit = $('input[type=submit]');
        if (form && submit) {
            var valueCache = []; //inputs value cache
            var allInputs = $('input, textarea, select')
                .not(':input[type=button], :input[type=submit], :input[type=reset]'); //ignore this elements because their value will never change

            // if input value changed -> enable submit
            allInputs.change(function(e) {
                submit.removeAttr('disabled');
            });

            /*
             * IF submit clicked first time -> initializes input values cache
             * next times -> compares cache and disable submit if nothing changed
             */
            submit.click(function (e) {
                //adding values to cache
                if (!valueCache.length) {
                    initCache(valueCache, allInputs);
                } else {
                    checkCache(valueCache, allInputs, submit, e);
                }
            });

        }
    });
})();