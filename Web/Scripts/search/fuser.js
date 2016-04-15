/*
    Setting up QueryParser and Finder classes
*/

(function () {
    'use strict';

    const USER_SELECTOR = '.tlr-usr-info'; //search by class tlr-usr-info
    const NAME_SELECTOR = '[data-userName]'; //search by data attribute data-userName
    const USER_SEARCH_FIELD_SELECTOR = 'input#emplSearch'; //search input with id #emplSearch
    const QUERY_PART_MIN_LENGTH = 4; //minimal length of query to search
    $(document).ready(function() {
        let finder = new Finder(USER_SELECTOR, NAME_SELECTOR, QUERY_PART_MIN_LENGTH);

        $(USER_SEARCH_FIELD_SELECTOR).on('input', function() {
            finder.findUsersByQuery($(this).val().toLowerCase());
        });
    });
})();