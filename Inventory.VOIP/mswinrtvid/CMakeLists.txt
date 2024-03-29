############################################################################
# CMakeLists.txt
# Copyright (C) 2016  Belledonne Communications, Grenoble France
#
############################################################################
#
# This program is free software; you can redistribute it and/or
# modify it under the terms of the GNU General Public License
# as published by the Free Software Foundation; either version 2
# of the License, or (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software
# Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
#
############################################################################

cmake_minimum_required(VERSION 3.0)
project(MSWINRTVID CXX)


option(ENABLE_STRICT "Build with strict compile options." YES)


macro(apply_compile_flags SOURCE_FILES)
	if(${SOURCE_FILES})
		set(options "")
		foreach(a ${ARGV})
			if(STRICT_OPTIONS_${a})
				string(REPLACE ";" " " options_${a} "${STRICT_OPTIONS_${a}}")
				set(options "${options} ${options_${a}}")
			endif()
		endforeach()
		if(options)
			set_source_files_properties(${${SOURCE_FILES}} PROPERTIES COMPILE_FLAGS "${options}")
		endif()
	endif()
endmacro()


if(LINPHONE_BUILDER_GROUP_EXTERNAL_SOURCE_PATH_BUILDERS)
	include("${EP_bctoolbox_CONFIG_DIR}/BcToolboxConfig.cmake")
else()
	find_package(BcToolbox REQUIRED)
endif()
if(LINPHONE_BUILDER_GROUP_EXTERNAL_SOURCE_PATH_BUILDERS)
	include("${EP_ortp_CONFIG_DIR}/ORTPConfig.cmake")
else()
	find_package(ORTP REQUIRED)
endif()
if(LINPHONE_BUILDER_GROUP_EXTERNAL_SOURCE_PATH_BUILDERS)
	include("${EP_ms2_CONFIG_DIR}/Mediastreamer2Config.cmake")
else()
	find_package(Mediastreamer2 REQUIRED)
endif()


include_directories(
	${MEDIASTREAMER2_INCLUDE_DIRS}
	${ORTP_INCLUDE_DIRS}
	${BCTOOLBOX_INCLUDE_DIRS}
)

set(STRICT_OPTIONS_CPP )
if(ENABLE_STRICT)
	list(APPEND STRICT_OPTIONS_CPP "/WX")
endif()

set(SOURCE_FILES
	"IVideoDispatcher.h"
	"IVideoRenderer.h"
	"LinkList.h"
	"MediaEngineNotify.cpp"
	"MediaEngineNotify.h"
	"MediaStreamSource.cpp"
	"MediaStreamSource.h"
	"mswinrtbackgrounddis.cpp"
	"mswinrtbackgrounddis.h"
	"mswinrtcap.cpp"
	"mswinrtcap.h"
	"mswinrtdis.cpp"
	"mswinrtdis.h"
	"mswinrtmediasink.cpp"
	"mswinrtmediasink.h"
	"mswinrtvid.cpp"
	"mswinrtvid.h"
	"RemoteHandle.cpp"
	"RemoteHandle.h"
	"Renderer.cpp"
	"Renderer.h"
	"ScopeLock.cpp"
	"ScopeLock.h"
	"SharedData.h"
	"VideoBuffer.h"
)
apply_compile_flags(SOURCE_FILES "CPP")
set(LIBS ${MEDIASTREAMER2_LIBRARIES} ${ORTP_LIBRARIES} ${BCTOOLBOX_LIBRARIES} mfplat.lib;mfuuid.lib)

add_library(mswinrtvid MODULE ${SOURCE_FILES})
set_target_properties(mswinrtvid PROPERTIES VERSION 0)
if(CMAKE_BUILD_TYPE STREQUAL "Debug")
	install(FILES ${CMAKE_CURRENT_BINARY_DIR}/Debug/libmswinrtvid.pdb
		DESTINATION lib/mediastreamer/plugins
		PERMISSIONS OWNER_READ OWNER_WRITE OWNER_EXECUTE GROUP_READ GROUP_EXECUTE WORLD_READ WORLD_EXECUTE
	)
endif()
set_target_properties(mswinrtvid PROPERTIES PREFIX "lib")
set_target_properties(mswinrtvid PROPERTIES VS_WINRT_COMPONENT TRUE)
target_link_libraries(mswinrtvid ${LIBS})

install(TARGETS mswinrtvid
	RUNTIME DESTINATION lib/mediastreamer/plugins
	LIBRARY DESTINATION lib/mediastreamer/plugins
	ARCHIVE DESTINATION lib/mediastreamer/plugins
	PERMISSIONS OWNER_READ OWNER_WRITE OWNER_EXECUTE GROUP_READ GROUP_EXECUTE WORLD_READ WORLD_EXECUTE
)
