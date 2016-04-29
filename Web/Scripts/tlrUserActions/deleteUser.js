(function() {
    const USER_INFO_SELECTOR = '.tlr-user-inner-info-trash';
    const HREF_TO_UPDATE_SELECTOR = 'a#delEmpl';
    const DATA_ATTR_OF_URL = 'reftodel';
    $(document).ready(function () {
        let usersToDelButtons = $(USER_INFO_SELECTOR);
        let hrefToUpdate = $(HREF_TO_UPDATE_SELECTOR);

        usersToDelButtons.click(function(e) {
            let urlToDelete = $(this).data(DATA_ATTR_OF_URL);
            hrefToUpdate.attr('href', urlToDelete);
        });
    });
})();