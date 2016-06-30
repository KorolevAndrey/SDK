// Copyright (C) NeoAxis Group Ltd. This is part of NeoAxis 3D Engine SDK.
#pragma once

#ifdef __cplusplus
extern "C" {
#endif

#if (defined(_WIN32) || defined(__WIN32__))
	#define PLATFORM_WINDOWS
#elif defined(__APPLE_CC__)
	#define PLATFORM_MACOS
#elif defined ANDROID
	#define PLATFORM_ANDROID
#else
	#error Platform is not supported.
#endif

/////////////////////////////////////////////////////////////////////////////////////////////////////////////

#ifdef PLATFORM_WINDOWS
	////VS2010 supported only. VS2011+ is not supported.
	//#if _MSC_VER <= 1600
		#define NATIVE_MEMORY_MANAGER_ENABLE
	//#endif
#endif

#ifdef PLATFORM_MACOS
	#define NATIVE_MEMORY_MANAGER_ENABLE
#endif

//#ifdef PLATFORM_ANDROID
//	#define NATIVE_MEMORY_MANAGER_ENABLE
//#endif

/////////////////////////////////////////////////////////////////////////////////////////////////////////////

#ifdef PLATFORM_WINDOWS
	#define NATIVEMEMORYMANAGER_EXPORT __declspec(dllexport)
	#define NATIVEMEMORYMANAGER_CALLING_CONVENTION _cdecl
#else
	#define NATIVEMEMORYMANAGER_EXPORT __attribute__ ((visibility("default")))
	#define NATIVEMEMORYMANAGER_CALLING_CONVENTION
#endif

typedef enum
{
	MemoryAllocationType_Renderer,
	MemoryAllocationType_Physics,
	MemoryAllocationType_SoundAndVideo,
	MemoryAllocationType_Utils,
	MemoryAllocationType_Other,

	MemoryAllocationType_Count,
} MemoryAllocationType;

NATIVEMEMORYMANAGER_EXPORT void MemoryManager_GetStatistics(MemoryAllocationType allocationType,
	int* allocatedMemory, int* allocationCount);

NATIVEMEMORYMANAGER_EXPORT void MemoryManager_GetCRTStatistics( int* allocatedMemory, int* allocationCount );

typedef void MemoryManager_GetAllocationInformationDelegate( 
	MemoryAllocationType allocationType, int size, const char* fileName, 
	int lineNumber, int allocationCount);

NATIVEMEMORYMANAGER_EXPORT void MemoryManager_GetAllocationInformation(
	MemoryManager_GetAllocationInformationDelegate* callback);

NATIVEMEMORYMANAGER_EXPORT void* Memory_Alloc( MemoryAllocationType allocationType,
	int size, const char* fileName, const int lineNumber );

NATIVEMEMORYMANAGER_EXPORT void* Memory_Realloc( MemoryAllocationType allocationType, 
	void* pointer, int newSize, const char* fileName, const int lineNumber );

NATIVEMEMORYMANAGER_EXPORT void Memory_Free( void* pointer );

NATIVEMEMORYMANAGER_EXPORT void* Memory_AllocAligned( MemoryAllocationType allocationType,
	int size, int align, const char* fileName, const int lineNumber );

NATIVEMEMORYMANAGER_EXPORT void Memory_FreeAligned( void* pointer );

NATIVEMEMORYMANAGER_EXPORT char* Memory_StrDup( const char* strSource );

/////////////////////////////////////////////////////////////////////////////////////////////////////////////

#ifdef NATIVE_MEMORY_MANAGER_ENABLE

#undef new
#undef delete
#undef malloc
#undef calloc
#undef realloc
#undef free

//redirect C functions

#define malloc(__s) Memory_Alloc( _DefinedMemoryAllocationType, __s, __FILE__, __LINE__ )
#define calloc(__c, __s) Memory_Alloc( _DefinedMemoryAllocationType, __c * __s, __FILE__, __LINE__ )
#define realloc(__p, __s) Memory_Realloc( _DefinedMemoryAllocationType, __p, __s, __FILE__, __LINE__ )
#define _recalloc(__p, __c, __s) XXX
#define _expand(__p, __s) XXX
#define free(__p) Memory_Free( __p )
#define _msize(__p) XXX
#define msize(__p) XXX

#define _aligned_malloc XXX
#define _aligned_realloc XXX
#define _aligned_free XXX
#define _aligned_offset_malloc XXX
#define _aligned_offset_realloc XXX
#define _aligned_offset_recalloc XXX

#define aligned_malloc XXX
#define aligned_realloc XXX
#define aligned_free XXX
#define aligned_offset_malloc XXX
#define aligned_offset_realloc XXX
#define aligned_offset_recalloc XXX

#define _strdup(__s) Memory_StrDup(__s)
#define _wcsdup XXX
#define _mbsdup XXX
#define _tempnam XXX
#define _wtempnam XXX
#define _fullpath XXX
#define _wfullpath XXX
#define _getcwd XXX
#define _wgetcwd XXX
#define _getdcwd XXX
#define _wgetdcwd XXX
#define _getdcwd_nolock XXX
#undef _wgetdcwd_nolock
#define _wgetdcwd_nolock XXX
#define _dupenv_s XXX
#define _wdupenv_s XXX
#define _dupenv XXX
#define _wdupenv XXX

#define strdup(__s) Memory_StrDup(__s)
#define wcsdup XXX
#define mbsdup XXX
#define tempnam XXX
#define wtempnam XXX
#define fullpath XXX
#define wfullpath XXX
#define getcwd XXX
#define wgetcwd XXX
#define getdcwd XXX
#define wgetdcwd XXX
#define getdcwd_nolock XXX
#define wgetdcwd_nolock XXX
#define dupenv_s XXX
#define wdupenv_s XXX
#define dupenv XXX
#define wdupenv XXX

#endif //NATIVE_MEMORY_MANAGER_ENABLE


#undef NATIVEMEMORYMANAGER_EXPORT

#ifdef __cplusplus
}
#endif
