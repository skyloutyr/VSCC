var client;
var chatWindow;
var chatButton;

var templates = {
    ["default"]: function (data) {
        if (data && data.name) {
            var ret = "&{template:default} "
            for (var propt in data) {
                if (data.hasOwnProperty(propt)) {
                    var val = data[propt];
                    if (typeof (val) === "string") {
                        ret += "{{" + propt + "=" + val + "}} ";
                    }
                    else if (val.key && val.value) {
                        ret += "{{" + val.key + "=" + val.value + "}} ";
                    }
                }
            }

            return ret;
        }

        return false;
    },

    ["description"]: function (data) {
        if (data && typeof (data) === "string") {
            return "&{template:desc} {{desc=" + data + "}}";
        }

        return false;
    },

    ["simple"]: function (data) {
        if (data && data.r1) {
            var ret = "&{template:simple} ";
            if (data.type) {
                ret += "{{" + data.type + "=1}} ";
            }
            else {
                if (data.r2) {
                    ret += "{{always=1}} ";
                }
                else {
                    ret += "{{normal=1}} ";
                }
            }

            if (data.name || data.rname) {
                if (data.name) {
                    ret += "{{rname=" + data.name + "}} ";
                }
                else {
                    ret += "{{rname=" + data.rname + "}} ";
                }
            }

            if (data.mod) {
                ret += "{{mod=" + data.mod + "}} ";
            }

            if (data.cname || data.charname) {
                if (data.charname) {
                    ret += "{{charname=" + data.charname + "}} ";
                }
                else {
                    ret += "{{charname=" + data.cname + "}} ";
                }
            }

            ret += "{{r1=" + data.r1 + "}} ";
            if (data.r2) {
                ret += "{{r2=" + data.r2 + "}} ";
            }

            return ret;
        }

        return false;
    },

    ["none"]: function(data) {
        if (data && (data.r || data.roll))
        {
            if (data.r) {
                return "/r " + data.r;
            }
            else {
                return "/r " + data.roll;
            }
        }
    },

    ["custom"]: function (data) {
        if (data && typeof (data) === "string") {
            return data;
        }
    }
};

var packets = {
    ["error"]: function (data) {
        if (data) {
            return {
                type: "error",
                data: {
                    code: data.code,
                    message: data.message
                }
            };
        }

        return false;
    }
};

/* Error code definitions:
 * -1: Catastrophical client error, terminate the connection. 
 * 0: No error.
 * 1: Invalid JSON sent/received.
 * 2: Malformed data.
 * 3: Invalid argument type supplied.
 * 4: Operation not supported.
 * 5: Operation not implemented.
 * 6: Permission error.
 * 7: Unknown packet type.
 * 127: Undefined client error
 */

function createClient(){
    try {
        client = new WebSocket("ws://localhost:23521");
        client.addEventListener("message", function (evt) {
            if (!parseServerInput(evt.data)) {
                console.log("Error parsing the input from the server! Check the server logs for an error message.");
            }
        });

        client.addEventListener("close", function (evt) {
            if (window.confirm("A server connection was terminated(closed)! Do you want to restart the client?")) {
                createClient();
            }
        });

        client.addEventListener("error", function (evt) {
            for (var propt in evt) {
                console.log(propt + ": " + evt[propt]);
            }

            if (window.confirm("A server connection was terminated(errored)! Do you want to restart the client?")) {
                createClient();
            }
        });

        return true;
    }
    catch (e) {
        client = null;
        window.alert("Unable to start a client!\nLikely your browser disallows unsecure web sockets.\nIf you want to use this feature you must enable unsecure web sockets.\nIf you are using Firefox open a new page and go to about:config. Agree to the liability factor. Search for network.websocket.allowInsecureFromHTTPS and set it to true.");
        return false;
    }
}

function validateSite() {
    var url = window.location.href;
    if (url.includes("app.roll20.net")) {
        var chat = document.getElementById("textchat-input");
        chatWindow = chat.getElementsByTagName("textarea")[0];
        chatButton = chat.getElementsByTagName("button")[0];
        if (chatWindow != null) {
            return true;
        }
    }

    return false;
}

function message(text) {
    if (text) {
        chatWindow.value = text;
        chatButton.click();
        return true;
    }

    return false;
}

function parseServerInput(json) {
    try {
        console.log(json);
        return handleServerCommand(JSON.parse(json));
    }
    catch (error) {
        send(packets["error"]({ code: 1, message: error }));
        return false;
    }
}

function assert(condition, errorCode, message) {
    if (!condition) {
        send(packets["error"]({ code: errorCode, message: message }));
        return false;
    }

    return true;
}

function assertParam(param, pname, expected) {
    return assert(param && typeof (param) === expected, 3, "Invalid argument parameter: " + pname + ", expected " + expected.toUpperCase() + ", got " + (param ? (typeof (param)).toUpperCase() : "NULL"));
}

function assertInteger(val, name) {
    return assert(val % 1 === 0, 3, "Invalid argument parameter: " + name + ", expected a non-decimal number.");
}

function handleServerCommand(data) {
    if (data.type && typeof (data.type) === "string") {
        switch (data.type.toLowerCase()) {
            case "close":
            case "exit":
            case "stop": {
                client.close(1000);
                client = null;
                return true;
            }

            case "message":
            case "chat_message":
            case "chatmessage": {
                if (assertParam(data.text, "text", "string")){
                    return message(data.text);
                } else {
                    return false;
                }
            }

            case "roll": {
                if (assertParam(data.numDice, "numDice", "number") && assertParam(data.numSides, "numSides", "number") && assertInteger(data.numDice, "numDice") && assertInteger(data.numSides, "numSides")) {
                    return message("/roll " + data.numDice + "d" + data.numSides);
                } else {
                    return false;
                }
            }

            case "r20command":
            case "command":
            case "roll20command":
            case "roll20_command":
            case "r20_command":
            case "command_r20":
            case "command_roll20":
            case "macro":
            case "advcommand":
            case "adv_command":
            case "advancedcommand":
            case "advanced_command": {
                if (assertParam(data.template, "template", "string") && assertParam(data.data, "data", "object")) {
                    if (templates[data.template.toLowerCase()]) {
                        if (message(templates[data.template.toLowerCase()](data.data))) {
                            return true;
                        }
                        else {
                            send(packets["error"]({ code: 4, message: "The data for the given template/macro was invalid. Check the validity of the data sent." }));
                            return false;
                        }
                    }
                    else {
                        send(packets["error"]({ code: 6, message: "A template " + data.template + " is not implemented/supported." }));
                        return false;
                    }
                }

                return false;
            }

            default: {
                send(packets["error"]({ code: 7, message: "Unknown packet type." }));
                return false;
            }
        }
    }
    else {
        send(packets["error"]({ code: 2, message: "No packet type specified." }));
        return false;
    }

    return false;
}

function send(text) {
    if (client && text) {
        try {
            client.send(typeof (text) === "string" ? text : JSON.stringify(text));
        } catch (error) {
            try {
                client.send(packets["error"]({ code: -1, message: toString(error) }));
            }
            finally {
                client = null;
            }

            return false;
        }

        return true;
    }

    return false;
}

function run() {
    if (validateSite() && createClient()) {
        console.log("Everything is setup correctly. You may now close the console and resume play.");
    }
}

if (window.confirm("Running VSCC roll20 client 1.0.1.\nBegin launch procedure?")) {
    run();
}