// inicalizar el Wizard
$('#smartwizard').smartWizard({
    selected: 0,
    anchorSettings: {
        anchorClickable: true,
        enableAllAnchors: true,
        markDoneStep: false,
    },
    theme: 'default',
    justified: true,
    transitionEffect: 'fade',
    showStepURLhash: false,
    autoAdjustHeight: true,
    toolbarSettings: {
        toolbarPosition: 'none',
        toolbarButtonPosition: 'none'
    },
    lang: {
        next: 'Siguiente',
        previous: 'Anterior'
    }    
});