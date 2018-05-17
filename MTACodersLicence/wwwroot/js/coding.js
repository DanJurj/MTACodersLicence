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

var editor = ace.edit("editor");
editor.setTheme("ace/theme/ambiance");
editor.session.setMode("ace/mode/c_cpp");
$runBtn = $('#runBtn');
changeLanguage();
function run() {
    var editor = ace.edit("editor");
    var code = editor.getSession().getValue();
    var code2 = code.split('"').join("\\\"");
    document.getElementById("code").value = code2;
}

//functie schimbare limbaj de programare
function changeLanguage() {
    var pos = document.getElementById("mode").options.selectedIndex;
    document.getElementById("language").value = pos;  //trimit la server varianta aleasa impreuna cu codul
    switch (pos) {
        case 0: editor.session.setMode("ace/mode/c_cpp");
            editor.setValue("#include<stdio.h>\n\
int main(void) \n\
{\n\
    int d; \n\
    scanf(\"%d\",&d); \n\
    printf(\"%d\",d); \n\
    return 0; \n\
}");
            break;

        case 1: editor.session.setMode("ace/mode/java");
            editor.setValue("\
public class Solution {\n\
\
    public static void main(String[] args) {\n\
    // Prints \"Hello, World\" to the terminal window.\n\
    System.out.println(\"Hello, World\");\n\
    }\n\
\
}");
            break;
        case 2: editor.session.setMode("ace/mode/python");
            editor.setValue("print(\"Hello World!\")");
            break;
        default:
            break;
    }
}

//functie schimbare tema
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

function toggleTime() {
    $('#time').fadeToggle('fast');
}


