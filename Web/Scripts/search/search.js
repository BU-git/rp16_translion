///<reference path="/Scripts/jquery-1.10.2.min.js"/>
'use strict';
/*
    Parses all text data from query string
*/
class QueryParser {
    constructor() {
        this._splitters = ['.', ',', '/', '\\', '_', ' '];
    }
    /**
     * Checks if there are splitters in query
     * @param {} query 
     * @returns {} 
     * if there is no splitters we should try find users names using whole query
     * else -> split query into parts 
     */
    checkIfContainsSplitters(query) {
        for (let i = 0; i < this._splitters.length; i++) {
            if (~query.indexOf(this._splitters[i])) //if found event one splitter -> returns true -> should split query into parts
                return true;
        }
        return false;
    }
    /**
     * Parses query into array of splitted parts
     * @param {} query 
     * @returns {} 
     * divided query
     */
    parseQuery(query) {
        let splitted = [query];
        if (!this.checkIfContainsSplitters(query))
            return splitted;
        for (let i = 0; i < this._splitters.length; i++) {
            let splitArr = splitted.slice();  //copying old splitted result
            splitted = []; //clearing old array to concat with next splitted
            for (let j = 0; j < splitArr.length; j++) 
                splitted = splitted.concat(splitArr[j].split(this._splitters[i])); //concating splitted and old array
        }
        return splitted.filter(elem => elem !== ''); //filter emplty strings
    }
};
/*
    Finds all users matches search query (uses QueryParser)
*/
class Finder {
    constructor(usrSelector, nameSelector) {
        this._parser = new QueryParser();
        this._users = $(usrSelector); //getting all users from 
        this._names = []; //this array's length should be equals user's array length
        this.initNames($(nameSelector));
        if (this._users.length !== this._names.length)
            console.log("ERROR: USER'S LENGTH IS NOT EQUALS TO NAMES LENGTH" + this._users.length +  " " + this._names.length);
    }
    /**
     * Get's users names from elements which were found by nameSelector
     * @param elems - HTML elements that contains usernames 
     * @returns {} 
     */
    initNames(elems) {
        let self = this; 
        elems.each(function () {
            self._names.push($(this).text().toLowerCase()); //saving user's names in lowerCase
        });
    }
    /**
     * Show element if it contains username 
     * @param {} username - username in element 
     * @param {} indx index of user in users array => name array index === user array index !!! 
     * @param {} parsedArr array with parsed splitted query string
     * @returns 
     * shows element with needed username
     */
    showIfContains(username, indx, parsedArr) {
        for (let i = 0; i < parsedArr.length; i++) { //foreach element in splitted query array
            if (~username.indexOf(parsedArr[i])) { //if username(element data) contains splitted element -> show element
                this._users.eq(indx).show();
            }
        }
    }
    /**
     * Find all users and show them using query entered by user in search field
     * @param {} query 
     * @returns shows found user and hides users that doesn't match query pattern
     */
    findUsersByQuery(query) {
        if (!query.length) { //if query is empty -> show all users
            this._users.show();
            return;
        }
        this._users.hide(); //hide all
        let parsedArr = this._parser.parseQuery(query); //parse query
        for (let i = 0; i < this._names.length; i++) { //for each userName trying to match query pattern
            this.showIfContains(this._names[i], i, parsedArr);
        }
    }
};