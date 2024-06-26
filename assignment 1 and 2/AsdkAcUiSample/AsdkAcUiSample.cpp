#include "rxregsvc.h"
#include "acutads.h"
// Simple acrxEntryPoint code. Normally intialization and cleanup
// (such as registering and removing commands) should be done here.
//
extern "C" AcRx::AppRetCode
acrxEntryPoint(AcRx::AppMsgCode msg, void* appId)
{
    switch (msg) {
    case AcRx::kInitAppMsg:
        // Allow application to be unloaded
        // Without this statement, AutoCAD will
        // not allow the application to be unloaded
        // except on AutoCAD exit.
        //
        acrxUnlockApplication(appId);
        // Register application as MDI aware. 
        // Without this statement, AutoCAD will
        // switch to SDI mode when loading the
        // application.
        //
        acrxRegisterAppMDIAware(appId);
        acutPrintf(L"\nExample Application Loaded");
        break;
    case AcRx::kUnloadAppMsg:
        acutPrintf(L"\nExample Application Unloaded");
        break;
    }
    return AcRx::kRetOK;
}