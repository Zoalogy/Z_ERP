function TypeValidation(type, word) {
    
    if (type == 1) {/// للكلمات والاسامي
        
        if (word.length < 8) {
            return " هذا الحقل قصير ";
        }
        else {
            return ' * ';
        }
    }
    else if (type == 2) {/// لكلمات المرور

    }
    else if (type == 3) {/// للبريد الاكتروني

    }
    else if (type == 4) {/// رقم الجوال
        
        if (word.length < 10 || !isNaN(word) == false) {
            return " رقم هاتف خاطئ ";
        }
        else {
            return ' * ';
        }
    }
    else if (type == 5) {/// رقم 

        if (!isNaN(word) == false) {
            return " رقم خاطئ ";
        }
        else {
            return ' * ';
        }
    }
}
function formvalid() {
    var x = true;
   
    if ($(".EmployeeValName").val() === "") {// الاسم
        
        $("#EmployeeValName").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        
        $("#EmployeeValName").text(TypeValidation(1, $(".EmployeeValName").val()));
        if (($("#EmployeeValName").text()) != ' * ') {
            x = false
        }
    }
    if ($(".EmployeevalAddress").val() === "") { // address
        $("#EmployeevalAddress").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#EmployeevalAddress").text(TypeValidation(1, $(".EmployeevalAddress").val()));
    }
    if ($(".EmployeeValPhone").val() === "") {// phone number
        
        $("#EmployeeValPhone").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        
        $("#EmployeeValPhone").text(TypeValidation(4, $(".EmployeeValPhone").val()));
    }
    if ($('#DepartmentID').val() === "") {  // department
        $("#DepartmentValID").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#DepartmentValID").text(' * ');
    }
    if ($('#JobID').val() === "") {  // // job
        $("#JobValID").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#JobValID").text(' * ');
    }
    if ($('#StatusID').val() === "") {  // // status
        $("#StatusValID").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#StatusValID").text(' * ');
    }
    if ($('#PointOfSaleID').val() === "") {  // // status
        $("#PosVal_TOupdate").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#PosVal_TOupdate").text(' * ');
    }

    return x;
}
function Expensesformvalid() {
    var x = true;
    if ($("#date-picker-example").val() === "") {// الاسم
        $("#date-picker-Valexample").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#date-picker-Valexample").text(' * ');
    }
    if ($(".ExpensesDescription").val() === "") { // address
        $("#ExpensesValDescription").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#ExpensesValDescription").text(' * ');
    }
    if ($('.ExpensesAmount').val() === "") {  // // status
        $("#ExpensesValAmount").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#ExpensesValAmount").text(' * ');
    }
    return x;
}
function DebtRecordsVal() {
    var x = true;
    if ($('.Employees_id').val() === "") {  // // الاسم
        $("#Employees_id_val").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#Employees_id_val").text(' * ');
    }
   
    if ($("#date-picker-example").val() === "") {// الاسم
        $("#date-picker-example_val").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#date-picker-example_val").text(' * ');
    }

    if ($(".DebtRecordsAmount").val() === "") { // address
        $("#DebtRecordsAmountVal").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        
        $("#DebtRecordsAmountVal").text(TypeValidation(5, $(".DebtRecordsAmount").val()));
        if (($("#DebtRecordsAmountVal").text()) != ' * ') {
            x = false;
        }
    }
    if ($('.DebtRecordsDescription').val() === "") {  // // status
        $("#DebtRecordsDescriptionVal").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        
        $("#DebtRecordsDescriptionVal").text(TypeValidation(1, $(".DebtRecordsDescription").val()));
        if (($("#DebtRecordsDescriptionVal").text()) != ' * ') {
            x = false
        }
    }
    return x;
}
function DedicationAllowanceVal() {
    var x = true;

    if ($(".SEmployee_ID").val() === "") {// الاسم

        $("#SEmployee_ID_val").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#SEmployee_ID_val").text(' * ');
    }
    
    if ($(".SAlownaceOrDeducation_ID").val() === "") {// الاسم

        $("#SAlownaceOrDeducation_ID_val").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#SAlownaceOrDeducation_ID_val").text(' * ');
    }

    if ($(".DedicationAllowanceListID").val() === "") {

        $("#DedicationAllowanceListID_val").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#DedicationAllowanceListID_val").text(' * ');
    }


    if ($('#EmpBatchValue').val() === "") {  // department
        $("#EmpBatchValue_val").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {
        $("#EmpBatchValue_val").text(TypeValidation(5, $("#EmpBatchValue").val()));
        if (($("#EmpBatchValue_val").text()) != ' * ') {
            x = false;
        }
    }
    return x;
}

///////////// departments

function Departmentsformvalid() {
    var x = true;
   
    if ($(".DepartmentNameAr").val() === "") { // address
        $("#DepartmentNameArVal").text('هذا الحقل مطلوب ');
        x = false;
    }
    else {Departmentsformvalid 
        $("#DepartmentNameArVal").text(' * ');
    }
   
    return x;
}

function AddPurchaseformvalid() {
    
    var x = true;
    if ($("#paymentMethodList option:selected").val() !== "3") {
        if ($('#txtItempurchasePrice').val() === "") {  // // status
            $("#txtItempurchasePrice").addClass('awad-v-e');
            $("#txtItempurchasePrice").removeClass('awad-v-s');
            x = false;
        }
        else {

            $("#txtItempurchasePrice").addClass('awad-v-s');
            $("#txtItempurchasePrice").removeClass('awad-v-e');
        }

        if ($("#txtItemName").val() === "") { // address
            $("#txtItemName").addClass('awad-v-e');
            $("#txtItemName").removeClass('awad-v-s');
            x = false;
        }
        else {
            $("#txtItemNameValid").text(' * ');
            $("#txtItemName").addClass('awad-v-s');
            $("#txtItemName").removeClass('awad-v-e');
        }
    }
    return x;
}
function AddPurchaseBillValid() {
    var x = true;
    

    if ($("#paymentMethodList option:selected").val() !== "3") { // address
        if ($('#OrderPaymentAmount').val() === "") {  // // status
            $("#OrderPaymentAmountValid").text('هذا الحقل مطلوب ');
            x = false;
        }
        else {
            $("#OrderPaymentAmountValid").text(' * ');
        }
        if ($('#chequeNumber').val() === "") {  // // status
            $("#chequeNumberValid").text('هذا الحقل مطلوب ');
            x = false;
        }
        else {
            $("#chequeNumberValid").text(' * ');
        }
        
    }
    else {
        $("#OrderPaymentAmountValid").text(' ');
    }

    return x;
}

function PaymentpurchasseBillValid() {
    var x = true;


    if ($("#paymentMethodList option:selected").val() != 3) { // address
        if ($('#OrderPaymentAmount').val() === "") {  // // status
            $("#OrderPaymentAmountValid").text('هذا الحقل مطلوب ');
            x = false;
        }
        else {
            $("#OrderPaymentAmountValid").text(' * ');
        }
        if ($('#chequeNumber').val() === "") {  // // status
            $("#chequeNumberValid").text('هذا الحقل مطلوب ');
            x = false;
        }
        else {
            $("#chequeNumberValid").text(' * ');
        }

    }
    else {
        $("#OrderPaymentAmountValid").text(' ');
    }

    return x;
}

function AddSuplierValid() {
    var x = true;
    if ($('#SuplierName').val() === "") {  // // status
        $("#SuplierName").addClass('awad-v-e');
        $("#SuplierName").removeClass('awad-v-s');
        
        x = false;
    }
    else {
        $("#SuplierName").removeClass('awad-v-e');
        $("#SuplierName").addClass('awad-v-s');
    }
    if ($('#SuplierPhone').val() === "" || $('#SuplierPhone').val().length < 10 ) {  // // status
        $("#SuplierPhone").addClass('awad-v-e');
        $("#SuplierPhone").removeClass('awad-v-s');
        x = false;
    }
    else {
        $("#SuplierPhone").removeClass('awad-v-e');
        $("#SuplierPhone").addClass('awad-v-s');
    }
    if ($('#SuplierBankAcount').val() === "") {  // // status
        $("#SuplierBankAcount").addClass('awad-v-e');
        $("#SuplierBankAcount").removeClass('awad-v-s');
        x = false;
    }
    else {
        $("#SuplierBankAcount").removeClass('awad-v-e');
        $("#SuplierBankAcount").addClass('awad-v-s');
    } 
    return x;
}
