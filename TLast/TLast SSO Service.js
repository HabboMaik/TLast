// ==UserScript==
// @name         TLast SSO Service
// @namespace    ric
// @version      0.1
// @description  Client by ric
// @author       ric
// @match        https://www.habbo.com.br/api/public/captcha*
// @grant        none
// ==/UserScript==

(function () {
    'use strict';

    let socket = new WebSocket("ws://localhost:27/ric");
    socket.onopen = () => {
        const urlParams = new URLSearchParams(window.location.search);
        if (urlParams.has("token") && urlParams.get("token") !== "") {
            socket.send(urlParams.get("token"));
        }
    }
})();