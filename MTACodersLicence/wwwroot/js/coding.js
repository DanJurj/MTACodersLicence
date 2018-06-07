var editor = ace.edit("editor");
editor.setTheme("ace/theme/ambiance");
initEditorMode();

function initEditorMode() {
    var language = $("#language option:first-child").text();
    switch (language) {
    case "C": editor.session.setMode("ace/mode/c_cpp");
        break;
    case "C++": editor.session.setMode("ace/mode/c_cpp");
        break;
    case "Java": editor.session.setMode("ace/mode/java");
        break;
    case "Python": editor.session.setMode("ace/mode/python");
        break;
    default: editor.session.setMode("ace/mode/c_cpp");
        break;
    }
}


function run() {
    var editor = ace.edit("editor");
    var code = editor.getSession().getValue();
    var code2 = code.split('"').join("\\\"");
    document.getElementById("code").value = code2;
    var language = $("#language option:selected").text();
    document.getElementById("languageName").value = language;
}

function hideCerinte() {
    document.getElementById("cerinte").style.display = 'none';
    document.getElementById("hideBtn").style.display = 'none';
    document.getElementById("showBtn").style.display = 'block';
    document.getElementById("codingPart").className = "col-md-11";
}

function showCerinte() {
    document.getElementById("cerinte").style.display = 'block';
    document.getElementById("hideBtn").style.display = 'block';
    document.getElementById("showBtn").style.display = 'none';
    document.getElementById("codingPart").className = "col-md-6";
}

function showHint() {
    document.getElementById("hint").style.display = 'block';
    document.getElementById("hintBtn").style.display = 'none';
}

// functie schimbare limbaj de programare
function changeLanguage() {
    var code = $('select[id=language]').val();
    editor.setValue(code);
    // change editor mode
    var language = $("#language option:selected").text();
    switch (language) {
    case "C": editor.session.setMode("ace/mode/c_cpp");
        break;
    case "C++": editor.session.setMode("ace/mode/c_cpp");
        break;
    case "Java": editor.session.setMode("ace/mode/java");
        break;
    case "Python": editor.session.setMode("ace/mode/python");
        break;
    default: editor.session.setMode("ace/mode/c_cpp");
        break;
    }
}

// functie schimbare tema
function changeTheme() {
    var pos = document.getElementById("theme").options.selectedIndex;
    switch (pos) {
        case 0: editor.setTheme("ace/theme/ambiance");
            break;
        case 1: editor.setTheme("ace/theme/chaos");
            break;
        case 2: editor.setTheme("ace/theme/chrome");
            break;
        case 3: editor.setTheme("ace/theme/clouds");
            break;
        case 4: editor.setTheme("ace/theme/cobalt");
            break;
        case 5: editor.setTheme("ace/theme/dracula");
            break;
        case 6: editor.setTheme("ace/theme/dreamweaver");
            break;
        case 7: editor.setTheme("ace/theme/eclipse");
            break;
        case 8: editor.setTheme("ace/theme/github");
            break;
        case 9: editor.setTheme("ace/theme/sqlserver");
            break;
        case 10: editor.setTheme("ace/theme/terminal");
            break;
        case 11: editor.setTheme("ace/theme/xcode");
            break;
        default:
            break;
    }
}

// functie citire fisier si incarcare in editor
function readSingleFile(evt) {
    var f = evt.target.files[0];
    if (f) {
        var r = new FileReader();
        r.onload = function (e) {
            var contents = e.target.result;
            editor.setValue(contents);
        }
        r.readAsText(f);
    } else {
        alert("Failed to load file");
    }
}

// functie ascundere timp
function toggleTime() {
    $('#time').fadeToggle('fast');
}

// functie salvare cod
function saveCode() {
    var code = editor.getValue();
    document.getElementById("savedCode").value = code;
    var language = $("#language option:selected").text();
    document.getElementById("languageNameSave").value = language;
}

